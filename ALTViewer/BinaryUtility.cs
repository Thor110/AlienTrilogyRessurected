/// <summary>
/// The BinaryUtility class is used to replace bytes at specified locations in a file.
/// </summary>
public static class BinaryUtility
{
    /// <summary>
    /// The ReplaceBndFrameWith8ByteAlignment method replaces a section of a BND file with new data, ensuring that the new data is aligned to 8-byte boundaries.
    /// </summary>
    public static void ReplaceBndFrameWith8ByteAlignment(string filePath, long offset, int lengthToReplace, byte[] newData)
    {
        int padding = (8 + ((int)offset + newData.Length % 8)) % 8;
        byte[] paddedNewData = new byte[newData.Length + padding];
        Array.Copy(newData, paddedNewData, newData.Length);
        ReplaceBytesWithResize(new List<(long, int, byte[])> { (offset, lengthToReplace, paddedNewData) }, filePath);
    }
    /// <summary>
    /// The ReplaceBytesWithResize method opens the relevant file to replace bytes in, then replaces a given amount of bytes and inserts the rest, extending the file length.
    /// </summary>
    public static void ReplaceBytesWithResize(List<(long Offset, int LengthToReplace, byte[] NewData)> edits, string filePath)
    {
        // Sort edits by offset to apply them sequentially
        edits = edits.OrderBy(e => e.Offset).ToList();
        byte[] original = File.ReadAllBytes(filePath);
        var result = new List<byte>();
        long currentPos = 0;
        long shift = 0;
        foreach (var edit in edits)
        {
            long adjustedOffset = edit.Offset + shift;
            // Copy unchanged data before this edit
            if (adjustedOffset > currentPos)
            {
                int length = (int)(adjustedOffset - currentPos);
                result.AddRange(original.Skip((int)currentPos).Take(length));
                currentPos += length;
            }
            // Skip the original data to be replaced
            currentPos += edit.LengthToReplace;
            // Insert new data
            result.AddRange(edit.NewData);
            // Update shift amount for later edits
            shift += edit.NewData.Length - edit.LengthToReplace;
        }
        // Append any remaining data after the last edit
        if (currentPos < original.Length)
        {
            result.AddRange(original.Skip((int)currentPos));
        }
        // Write back to file
        File.WriteAllBytes(filePath, result.ToArray());
    }
    /// <summary>
    /// The ReplaceByte method opens the relevant file to replace a byte in.
    /// </summary>
    /// <param name="offset">The address at which to replace a byte.</param>
    /// <param name="value">The byte to write as a replacement.</param>
    /// <param name="filename">The BinaryWriter Object.</param>
    public static void ReplaceByte(long offset, byte value, string filename)
    {
        using var fs = new FileStream(filename, FileMode.Open, FileAccess.Write, FileShare.None);
        fs.Seek(offset, SeekOrigin.Begin);
        fs.WriteByte(value);
    }
    /// <summary>
    /// The ReplaceByte method opens the relevant file to replace multiple bytes in a sequence.
    /// </summary>
    /// <param name="replacements">The byte to replace and the address at which to replace it.</param>
    /// <param name="filename">The BinaryWriter Object.</param>
    public static void ReplaceBytes(List<Tuple<long, byte[]>> replacements, string filename)
    {
        using (var stream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
        {
            using (var reader = new BinaryReader(stream))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    Replace(reader, writer, replacements);
                }
            }
        }
    }
    /// <summary>
    /// The Replace method is called by the ReplaceBytes method.
    /// </summary>
    /// <param name="reader">The BinaryReader Object.</param>
    /// <param name="writer">The BinaryWriter Object.</param>
    /// <param name="replacements">The bytes to replace and the address at which to replace them.</param>
    public static void Replace(BinaryReader reader, BinaryWriter writer, IEnumerable<Tuple<long, byte[]>> replacements)
    {
        byte[] bytes = new byte[reader.BaseStream.Length];
        reader.BaseStream.Position = 0;
        reader.Read(bytes, 0, bytes.Length);
        foreach (var replacement in replacements)
        {
            Array.Copy(replacement.Item2, 0, bytes, replacement.Item1, replacement.Item2.Length);
        }
        writer.BaseStream.Position = 0;
        writer.BaseStream.SetLength(0);
        writer.Write(bytes);
    }
}