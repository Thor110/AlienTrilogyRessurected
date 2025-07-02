namespace ALTViewer
{
    public static class DetectDimensions
    {
        // Auto-detect dimensions based on the total pixel count in the image data
        public static (int w, int h) AutoDetectDimensions(string lastSelectedFile, int SelectedIndex, int FrameIndex)
        {
            // TODO : setup all the extra switch statements
            int w = 0;
            int h = 0;
            if (lastSelectedFile == "FLAME") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0: w = 68; h = 70; break; // CORRECT
                    case 1: w = 64; h = 69; break; // CORRECT
                    case 2:
                        switch(FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 64; h = 69; break; // CORRECT
                            case 1: w = 64; h = 70; break;
                            case 2: w = 64; h = 71; break;
                        }
                        break;
                }
            }
            else if (lastSelectedFile == "MM9") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0: w = 40; h = 65; break; // CORRECT
                    case 1:
                        switch (FrameIndex) // COMPLETE
                        {
                            case 0: w = 40; h = 86; break; // CORRECT
                            case 1: w = 40; h = 68; break; // CORRECT
                            case 2: w = 40; h = 65; break; // CORRECT
                        }
                        break;
                    case 2:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 76; h = 83; break; // CORRECT
                            case 1: w = 76; h = 83; break; //??
                            case 2: w = 76; h = 83; break; //??
                        }
                        break;
                }
            }
            else if (lastSelectedFile == "PULSE") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0: w = 84; h = 65; break;
                    case 1:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 72; h = 53; break; // CORRECT
                            case 1: w = 72; h = 53; break;
                        }
                        break;
                    case 2:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 84; h = 85; break; // CORRECT
                            case 1: w = 84; h = 85; break;
                            case 2: w = 84; h = 85; break;
                        }
                        break;
                    case 3:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 92; h = 74; break; // CORRECT
                            case 1: w = 92; h = 74; break;
                            case 2: w = 92; h = 74; break;
                        }
                        break;
                }
            }
            else if (lastSelectedFile == "SHOTGUN") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0: w = 84; h = 77; break;
                    case 1:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 72; h = 101; break; // CORRECT
                            case 1: w = 72; h = 101; break;
                            case 2: w = 72; h = 101; break;
                            case 3: w = 72; h = 101; break;
                            case 4: w = 72; h = 101; break;
                        }
                        break;
                }
            }
            else if (lastSelectedFile == "SMART") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0: w = 120; h = 52; break;
                    case 1:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 124; h = 108; break; // CORRECT
                            case 1: w = 124; h = 108; break;
                        }
                        break;
                    case 2:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 128; h = 50; break; // CORRECT
                            case 1: w = 128; h = 50; break;
                            case 2: w = 128; h = 50; break;
                        }
                        break;
                }
            }
            else if (lastSelectedFile == "BAMBI") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0: w = 32; h = 59; break;
                    case 4: w = 32; h = 69; break;
                    case 5: w = 32; h = 70; break;
                    case 1: w = 40; h = 64; break;
                    case 2: w = 44; h = 72; break;
                    case 8: w = 44; h = 67; break;
                    case 9: w = 96; h = 33; break;
                    case 3: w = 92; h = 67; break;
                    case 6:
                    case 7: w = 64; h = 69; break;
                }
            }
            else if (lastSelectedFile == "BURSTER") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0: w = 40; h = 48; break;
                    case 1: w = 32; h = 30; break;
                    case 2: w = 52; h = 61; break;
                    case 3: w = 36; h = 35; break;
                    case 4: w = 84; h = 61; break;
                    case 5: w = 52; h = 40; break;
                    case 6: w = 60; h = 42; break;
                    case 7: w = 60; h = 34; break;
                    case 8: w = 56; h = 39; break;
                    case 9: w = 56; h = 38; break;
                    case 10: w = 48; h = 31; break;
                }
            }
            else if (lastSelectedFile == "COLONIST") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0:
                    case 1: w = 32; h = 56; break;
                    case 5:
                    case 6: w = 32; h = 60; break;
                    case 12:
                    case 13: w = 32; h = 59; break;
                    case 20:
                    case 21: w = 32; h = 41; break;
                    case 3: w = 40; h = 56; break;
                    case 9:
                    case 11:
                    case 15: w = 40; h = 60; break;
                    case 17: w = 40; h = 59; break;
                    case 2:
                    case 4: w = 28; h = 57; break;
                    case 7:
                    case 8: w = 24; h = 60; break;
                    case 10:
                    case 14: w = 24; h = 58; break;
                    case 18:
                    case 22: w = 24; h = 42; break;
                    case 16: w = 36; h = 59; break;
                    case 19: w = 36; h = 42; break;
                }
            }
            else if (lastSelectedFile == "DOG") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0: w = 88; h = 101; break;
                    case 1: w = 88; h = 52; break;
                    case 2: w = 84; h = 98; break;
                    case 3: w = 64; h = 92; break;
                    case 4: w = 56; h = 78; break;
                    case 5: w = 80; h = 79; break;
                    case 6: w = 100; h = 82; break;
                    case 7: w = 104; h = 86; break;
                    case 8: w = 60; h = 86; break;
                    case 9:
                    case 10: w = 60; h = 92; break;
                }
            }
            else if (lastSelectedFile == "DOGCEIL") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0: w = 120; h = 29; break;
                    case 1: w = 112; h = 28; break;
                    case 2: w = 80; h = 113; break;
                    case 3: w = 120; h = 44; break;
                    case 4: w = 92; h = 77; break;
                }
            }
            else if (lastSelectedFile == "EGGS") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0:
                    case 1: w = 48; h = 56; break;
                    case 2: w = 48; h = 63; break;
                    case 3: w = 52; h = 30; break;
                    case 4: w = 48; h = 62; break;
                    case 5: w = 48; h = 57; break;
                }
            }
            else if (lastSelectedFile == "FINGERS") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0: w = 120; h = 43; break;
                    case 1: w = 120; h = 45; break;
                }
            }
            else if (lastSelectedFile == "GUARD") // INCOMPLETE
            {
                switch (    SelectedIndex)
                {
                    case 0: w = 56; h = 120; break;
                    case 1: w = 44; h = 113; break;
                    case 2: w = 32; h = 115; break;
                    case 3: w = 56; h = 119; break;
                    case 4: w = 56; h = 115; break;
                    case 5: w = 56; h = 116; break;
                    case 6: w = 64; h = 118; break;
                    case 7: w = 56; h = 114; break;
                }
            }
            else if (lastSelectedFile == "HANDLER") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0: w = 64; h = 101; break;
                    case 1: w = 52; h = 101; break;
                    case 2: w = 48; h = 102; break;
                    case 3: w = 56; h = 103; break;
                    case 4: w = 68; h = 102; break;
                    case 5: w = 56; h = 104; break;
                    case 6: w = 68; h = 100; break;
                    case 7: w = 48; h = 100; break;
                    case 8: w = 44; h = 100; break;
                }
            }
            else if (lastSelectedFile == "HUGGER") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0: w = 88; h = 19; break;
                    case 1: w = 92; h = 30; break;
                    case 2: w = 124; h = 26; break;
                    case 3: w = 92; h = 30; break;
                    case 4: w = 48; h = 37; break;
                    case 5: w = 64; h = 32; break;
                    case 6: w = 76; h = 40; break;
                    case 7: w = 48; h = 62; break;
                }
            }
            else if (lastSelectedFile == "QUEEN") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0: w = 148; h = 166; break;
                    case 1: w = 124; h = 178; break;
                    case 2: w = 132; h = 154; break;
                    case 3: w = 148; h = 202; break;
                    case 4: w = 208; h = 189; break;
                    case 5: w = 128; h = 176; break;
                    case 6: w = 168; h = 183; break;
                    case 7: w = 148; h = 146; break;
                    case 8: w = 136; h = 182; break;
                    case 9: w = 152; h = 182; break;
                    case 10: w = 128; h = 173; break;
                    case 11: w = 140; h = 187; break;
                }
            }
            else if (lastSelectedFile == "SOLDIER") // INCOMPLETE [GET HEIGHT VALUES]
            {
                switch (SelectedIndex)
                {
                    case 0: w = 48; h = 130; break;
                    case 1: w = 44; h = 130; break;
                    case 2: w = 48; h = 130; break;
                    case 3: w = 60; h = 130; break;
                    case 4: w = 60; h = 130; break;
                    case 5: w = 60; h = 130; break;
                    case 6: w = 40; h = 130; break;
                    case 7: w = 68; h = 130; break;
                }
            }
            else if (lastSelectedFile == "SYNTH") // INCOMPLETE [GET HEIGHT VALUES]
            {
                switch (SelectedIndex)
                {
                    case 0: w = 84; h = 130; break;
                    case 1: w = 44; h = 130; break;
                    case 2: w = 72; h = 130; break;
                    case 3: w = 88; h = 130; break;
                    case 4: w = 40; h = 130; break;
                    case 5: w = 72; h = 130; break;
                    case 6: w = 68; h = 130; break;
                    case 7: w = 68; h = 130; break;
                    case 8: w = 64; h = 130; break;
                    case 9: w = 72; h = 130; break;
                    case 10: w = 64; h = 130; break;
                    case 11: w = 64; h = 130; break;
                    case 12: w = 72; h = 130; break;
                    case 13: w = 40; h = 130; break;
                    case 14: w = 100; h = 130; break;
                    case 15: w = 96; h = 130; break;
                    case 16: w = 68; h = 130; break;
                    case 17: w = 84; h = 130; break;
                    case 18: w = 80; h = 130; break;
                    case 19: w = 64; h = 130; break;
                }
            }
            else if (lastSelectedFile == "WAR") // INCOMPLETE [GET HEIGHT VALUES]
            {
                switch (SelectedIndex)
                {
                    case 0: w = 76; h = 130; break;
                    case 1: w = 48; h = 130; break;
                    case 2: w = 60; h = 130; break;
                    case 3: w = 56; h = 130; break;
                    case 4: w = 88; h = 130; break;
                    case 5: w = 112; h = 130; break;
                    case 6: w = 76; h = 130; break;
                    case 7: w = 60; h = 130; break;
                    case 8: w = 52; h = 130; break;
                    case 9: w = 52; h = 130; break;
                }
            }
            else if (lastSelectedFile == "WARCEIL") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0: w = 112; h = 81; break;
                    case 1: w = 128; h = 38; break;
                    case 2: w = 128; h = 38; break;
                    case 3: w = 120; h = 38; break;
                    case 4: w = 136; h = 41; break;
                }
            }
            return (w, h);
        }
    }
}