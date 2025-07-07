namespace ALTViewer
{
    public static class DetectDimensions
    {
        public static int[] TransparencyValues(string fileDirectory, int index)
        {
            string[] excluded = { "FLAME", "FONT1GFX", "LEGAL", "LOGOSGFX", "MM9", "PULSE", "SHOTGUN", "SMART",
                "BAMBI", "BURSTER", "COLONIST", "DOG", "DOGCEIL", "EGGS", "FINGERS", "GUARD", "HANDLER", "HUGGER", "QUEEN", "SOLDIER", "SYNTH", "WAR", "WARCEIL"};
            int[] values = null!;
            if (excluded.Any(e => fileDirectory.Contains(e))) { values = new int[] { 0 }; }
            if (fileDirectory.Contains("EXPLGFX"))
            {
                if (index == 0) { values = new int[] { 0, 16, 41 }; }
                if (index == 1) { values = new int[] { 0 }; }
            }
            if (fileDirectory.Contains("NME")) { if (index == 0) { values = new int[] { 0 }; } }
            //string directory = "";
            if (fileDirectory.Contains("SECT"))
            {
                switch (fileDirectory.Substring(fileDirectory.Length - 10, 3)) // ###GFX.B16
                {
                    //SECT11
                    case "111":
                        switch(index)
                        {
                            case 0:
                            case 2:
                            case 4:
                                values = new int[] { 255 };
                                break;
                            case 1:
                            case 3:
                                values = new int[] { 0 };
                                break;
                        }
                        break;
                    case "112":
                        switch (index)
                        {
                            case 0:
                                values = null!;
                                break;
                            case 1:
                            case 3:
                                values = new int[] { 0 };
                                break;
                            case 2:
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    case "113":
                        switch (index)
                        {
                            case 0:
                            case 2:
                            case 4:
                                values = new int[] { 255 };
                                break;
                            case 1:
                                values = new int[] { 0 };
                                break;
                            case 3:
                                values = new int[] { 0 };
                                break;
                        }
                        break;
                    case "114":
                        switch (index)
                        {
                            case 0:
                            case 2:
                            case 4:
                                values = new int[] { 255 };
                                break;
                            case 1:
                            case 3:
                                values = new int[] { 0 };
                                break;
                        }
                        break;
                    case "115":
                        switch (index)
                        {
                            case 0:
                            case 2:
                            case 4:
                                values = new int[] { 255 };
                                break;
                            case 1:
                            case 3:
                                values = new int[] { 0 };
                                break;
                        }
                        break;
                    case "121":
                        switch (index)
                        {
                            case 0:
                            case 2:
                            case 4:
                                values = new int[] { 255 };
                                break;
                            case 1:
                            case 3:
                                values = new int[] { 0 };
                                break;
                        }
                        break;
                    case "122":
                        switch (index)
                        {
                            case 0:
                                values = new int[] { 255 };
                                break;
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                                values = null!;
                                break;
                        }
                        break;
                    case "131":
                        values = null!;
                        break;
                    //SECT12
                    case "141":
                        switch (index)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                                values = null!;
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    case "154":
                        switch (index)
                        {
                            case 0:
                                values = new int[] { 255 };
                                break;
                            case 1:
                            case 2:
                            case 3:
                                values = null!;
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    case "155":
                        switch (index)
                        {
                            case 0:
                                values = new int[] { 255 };
                                break;
                            case 1:
                            case 2:
                            case 3:
                                values = null!;
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    case "161":
                        switch (index)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                                values = null!;
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    case "162":
                        switch (index)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                                values = null!;
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    //SECT21
                    case "211":
                        values = null!;
                        break;
                    case "212":
                        values = null!;
                        break;
                    case "213":
                        switch (index)
                        {
                            case 0:
                                values = new int[] { 255 };
                                break;
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                                values = null!;
                                break;
                        }
                        break;
                    case "222":
                        switch (index)
                        {
                            case 0:
                            case 1:
                            case 3:
                            case 4:
                                values = null!;
                                break;
                            case 2:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    case "231":
                        values = null!;
                        break;
                    case "232":
                        values = null!;
                        break;
                    //SECT22
                    case "242":
                        values = null!;
                        break;
                    case "243":
                        values = null!;
                        break;
                    case "262":
                        values = null!;
                        break;
                    case "263":
                        switch (index)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                                values = null!;
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    //SECT31
                    case "311":
                        switch (index)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                                values = null!;
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    case "321":
                        switch (index)
                        {
                            case 0:
                                values = new int[] { 255 };
                                break;
                            case 1:
                            case 2:
                            case 3:
                                values = null!;
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    case "322":
                        switch (index)
                        {
                            case 0:
                                values = new int[] { 255 };
                                break;
                            case 1:
                            case 2:
                            case 3:
                                values = null!;
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    case "323":
                        switch (index)
                        {
                            case 0:
                                values = new int[] { 255 };
                                break;
                            case 1:
                            case 2:
                            case 3:
                                values = null!;
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    case "324":
                        switch (index)
                        {
                            case 0:
                                values = new int[] { 255 };
                                break;
                            case 1:
                                values = new int[] { 0 };
                                break;
                            case 2:
                            case 3:
                                values = null!;
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    case "325":
                        switch (index)
                        {
                            case 0:
                                values = new int[] { 255 };
                                break;
                            case 1:
                            case 2:
                            case 3:
                                values = null!;
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    case "331":
                        values = null!;
                        break;
                    //SECT32
                    case "351":
                        switch (index)
                        {
                            case 0:
                            case 1:
                            case 3:
                                values = null!;
                                break;
                            case 2:
                                values = new int[] { 255 };
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    case "352":
                        switch (index)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                                values = null!;
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    case "353":
                        switch (index)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                                values = null!;
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    case "361":
                        values = null!;
                        break;
                    case "371":
                        switch (index)
                        {
                            case 0:
                            case 1:
                            case 3:
                                values = null!;
                                break;
                            case 2:
                                values = new int[] { 255 };
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    case "381":
                        switch (index)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                                values = null!;
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    case "391":
                        values = null!;
                        break;
                    //SECT90
                    case "900":
                        switch (index)
                        {
                            case 0:
                                values = new int[] { 255 };
                                break;
                            case 1:
                                values = new int[] { 0 };
                                break;
                            case 2:
                                values = null!;
                                break;
                            case 3:
                                values = new int[] { 0 };
                                break;
                            case 4:
                                values = null!;
                                break;
                        }
                        break;
                    case "901":
                        values = null!;
                        break;
                    case "902":
                        switch (index)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                                values = null!;
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    case "903":
                        switch (index)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                                values = null!;
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    case "904":
                        switch (index)
                        {
                            case 0:
                                values = new int[] { 255 };
                                break;
                            case 1:
                            case 2:
                            case 3:
                                values = null!;
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    case "905":
                        switch (index)
                        {
                            case 0:
                                values = new int[] { 255 };
                                break;
                            case 1:
                            case 2:
                            case 3:
                                values = null!;
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    case "906":
                        values = null!;
                        break;
                    case "907":
                        values = null!;
                        break;
                    case "908":
                        switch (index)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                                values = null!;
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                    case "909":
                        switch (index)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                                values = null!;
                                break;
                            case 4:
                                values = new int[] { 255 };
                                break;
                        }
                        break;
                }
            }
            if (fileDirectory.Contains("PNL")) { if (index == 0) { values = new int[] { 16 }; } }
            return values;
        }
        // Auto-detect dimensions based on the total pixel count in the image data
        public static (int w, int h) AutoDetectDimensions(string lastSelectedFile, int SelectedIndex, int FrameIndex)
        {
            int w = 0, h = 0;
            switch (lastSelectedFile)
            {
                case "FLAME": // COMPLETE
                    switch (SelectedIndex)
                    {
                        case 0: w = 68; h = 72; break; // CORRECT
                        case 1: w = 64; h = 72; break; // CORRECT
                        case 2:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 64; h = 72; break; // CORRECT
                                case 1: w = 72; h = 88; break; // CORRECT
                                case 2: w = 92; h = 100; break; // CORRECT
                            }
                            break;
                    }
                    break;
                case "MM9": // COMPLETE
                    switch (SelectedIndex)
                    {
                        case 0: w = 40; h = 68; break; // CORRECT
                        case 1:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 40; h = 88; break; // CORRECT
                                case 1: w = 40; h = 72; break; // CORRECT
                                case 2: w = 40; h = 68; break; // CORRECT
                            }
                            break;
                        case 2:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 76; h = 84; break; // CORRECT
                                case 1: w = 104; h = 108; break; // CORRECT
                                case 2: w = 84; h = 76; break; // CORRECT
                            }
                            break;
                    }
                    break;
                case "PULSE": // COMPLETE
                    switch (SelectedIndex)
                    {
                        case 0: w = 84; h = 68; break; // CORRECT
                        case 1:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 72; h = 56; break; // CORRECT
                                case 1: w = 80; h = 64; break; // CORRECT
                            }
                            break;
                        case 2:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 84; h = 88; break; // CORRECT
                                case 1: w = 84; h = 92; break; // CORRECT
                                case 2: w = 88; h = 92; break; // CORRECT
                            }
                            break;
                        case 3:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 92; h = 76; break; // CORRECT
                                case 1: w = 128; h = 76; break; // CORRECT
                                case 2: w = 124; h = 76; break; // CORRECT
                            }
                            break;
                    }
                    break;
                case "SHOTGUN": // COMPLETE
                    switch (SelectedIndex)
                    {
                        case 0: w = 84; h = 80; break; // CORRECT
                        case 1:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 72; h = 104; break; // CORRECT
                                case 1: w = 76; h = 92; break;  // CORRECT
                                case 2: w = 72; h = 84; break; // CORRECT
                                case 3: w = 64; h = 84; break; // CORRECT
                                case 4: w = 76; h = 92; break; // CORRECT
                            }
                            break;
                    }
                    break;
                case "SMART": // COMPLETE
                    switch (SelectedIndex)
                    {
                        case 0: w = 120; h = 56; break; // CORRECT
                        case 1:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 124; h = 112; break; // CORRECT
                                case 1: w = 128; h = 108; break; // CORRECT
                            }
                            break;
                        case 2:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 128; h = 52; break; // CORRECT
                                case 1: w = 124; h = 84; break; // CORRECT
                                case 2: w = 116; h = 116; break; // CORRECT
                            }
                            break;
                    }
                    break;
                case "BAMBI": // COMPLETE
                    switch (SelectedIndex)
                    {
                        case 0:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 1: w = 32; h = 60; break; // CORRECT
                                case 2: w = 36; h = 68; break; // CORRECT
                                case 3: w = 32; h = 76; break; // CORRECT
                                case 4: w = 52; h = 80; break; // CORRECT
                                case 5: w = 36; h = 80; break; // CORRECT
                                case 6: w = 32; h = 68; break; // CORRECT
                                case 7: w = 32; h = 72; break; // CORRECT
                            }
                            break;
                        case 1:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 40; h = 68; break; // CORRECT
                                case 1: w = 52; h = 68; break; // CORRECT
                                case 2: w = 72; h = 72; break; // CORRECT
                                case 3: // CORRECT
                                case 4: w = 64; h = 84; break; // CORRECT
                                case 5: w = 80; h = 76; break; // CORRECT
                            }
                            break;
                        case 2:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 44; h = 76; break; // CORRECT
                                case 1: w = 52; h = 72; break; // CORRECT
                                case 2: w = 72; h = 72; break; // CORRECT
                                case 3: w = 68; h = 84; break; // CORRECT
                                case 4: w = 72; h = 96; break; // CORRECT
                                case 5: w = 84; h = 84; break; // CORRECT
                            }
                            break;
                        case 3:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 92; h = 68; break; // CORRECT
                                case 1: w = 84; h = 76; break; // CORRECT
                                case 2: w = 116; h = 80; break; // CORRECT
                                case 3: w = 92; h = 72; break; // CORRECT
                                case 4: w = 76; h = 72; break; // CORRECT
                                case 5: w = 72; h = 80; break; // CORRECT
                                case 6: w = 92; h = 80; break; // CORRECT
                                case 7: w = 120; h = 72; break; // CORRECT
                            }
                            break;
                        case 4:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 4: w = 32; h = 72; break; // CORRECT
                                case 1: w = 40; h = 76; break; // CORRECT
                                case 2: // CORRECT
                                case 6: w = 36; h = 84; break; // CORRECT
                                case 3: w = 32; h = 76; break; // CORRECT
                                case 5: w = 40; h = 80; break; // CORRECT
                                case 7: w = 28; h = 80; break; // CORRECT
                            }
                            break;
                        case 5:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 4: w = 32; h = 72; break; // CORRECT
                                case 1: w = 44; h = 76; break; // CORRECT
                                case 2: w = 40; h = 80; break; // CORRECT
                                case 3: // CORRECT
                                case 7: w = 32; h = 76; break; // CORRECT
                                case 5: // CORRECT
                                case 6: w = 40; h = 84; break; // CORRECT
                            }
                            break;
                        case 6:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 64; h = 72; break; // CORRECT
                                case 1: w = 68; h = 76; break; // CORRECT
                                case 2: w = 92; h = 80; break; // CORRECT
                                case 3: w = 72; h = 72; break; // CORRECT
                                case 4: w = 64; h = 72; break; // CORRECT
                                case 5: w = 56; h = 80; break; // CORRECT
                                case 6: w = 60; h = 80; break; // CORRECT
                                case 7: w = 88; h = 76; break; // CORRECT
                            }
                            break;
                        case 7:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 64; h = 72; break; // CORRECT
                                case 1: w = 76; h = 76; break; // CORRECT
                                case 2: w = 80; h = 80; break; // CORRECT
                                case 3: w = 68; h = 76; break; // CORRECT
                                case 4: w = 52; h = 72; break; // CORRECT
                                case 5: w = 68; h = 84; break; // CORRECT
                                case 6: w = 84; h = 84; break; // CORRECT
                                case 7: w = 88; h = 76; break; // CORRECT
                            }
                            break;
                        case 8:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 44; h = 68; break; // CORRECT
                                case 1: w = 72; h = 84; break; // CORRECT
                                case 2: w = 84; h = 88; break; // CORRECT
                                case 3: w = 76; h = 76; break; // CORRECT
                                case 4: w = 92; h = 56; break; // CORRECT
                                case 5: w = 116; h = 36; break; // CORRECT
                            }
                            break;
                        case 9:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 48; h = 68; break; // CORRECT
                                case 1: w = 56; h = 76; break; // CORRECT
                                case 2: w = 64; h = 76; break; // CORRECT
                                case 3: w = 68; h = 72; break; // CORRECT
                                case 4: w = 84; h = 112; break; // CORRECT
                                case 5: w = 96; h = 36; break; // CORRECT
                            }
                            break;
                    }
                    break;
                case "BURSTER": // COMPLETE
                    switch (SelectedIndex)
                    {
                        case 0:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 40; h = 48; break; // CORRECT
                                case 1: w = 36; h = 32; break; // CORRECT
                                case 2: w = 32; h = 28; break; // CORRECT
                                case 3: w = 44; h = 40; break; // CORRECT
                            }
                            break;
                        case 1:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 32; h = 32; break; // CORRECT
                                case 1: w = 24; h = 40; break; // CORRECT
                                case 2: w = 32; h = 28; break; // CORRECT
                                case 3: w = 28; h = 36; break; // CORRECT
                                case 4: w = 40; h = 40; break; // CORRECT
                                case 5: w = 40; h = 36; break; // CORRECT
                                case 6: w = 36; h = 28; break; // CORRECT
                                case 7: w = 40; h = 32; break; // CORRECT
                            }
                            break;
                        case 2:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 52; h = 64; break; // CORRECT
                                case 1: // CORRECT
                                case 2: w = 72; h = 56; break; // CORRECT
                                case 3: w = 84; h = 52; break; // CORRECT
                            }
                            break;
                        case 3:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 36; h = 36; break; // CORRECT
                                case 1: w = 72; h = 44; break; // CORRECT
                                case 2: w = 100; h = 44; break; // CORRECT
                                case 3: w = 112; h = 44; break; // CORRECT
                            }
                            break;
                        case 4:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 84; h = 64; break; // CORRECT
                                case 1: w = 108; h = 64; break; // CORRECT
                                case 2: w = 128; h = 48; break; // CORRECT
                                case 3: w = 108; h = 48; break; // CORRECT
                                case 4: w = 120; h = 48; break; // CORRECT
                            }
                            break;
                        case 5:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 52; h = 40; break; // CORRECT
                                case 1: w = 84; h = 60; break; // CORRECT
                                case 2: w = 108; h = 56; break; // CORRECT
                                case 3: w = 120; h = 60; break; // CORRECT
                                case 4: w = 120; h = 56; break; // CORRECT
                            }
                            break;
                        case 6:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 60; h = 44; break; // CORRECT
                                case 1: w = 100; h = 44; break; // CORRECT
                                case 2: w = 72; h = 40; break; // CORRECT
                                case 3: w = 60; h = 40; break; // CORRECT
                            }
                            break;
                        case 7:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 60; h = 36; break; // CORRECT
                                case 1: w = 88; h = 28; break; // CORRECT
                                case 2: w = 68; h = 32; break; // CORRECT
                                case 3: w = 64; h = 32; break; // CORRECT
                            }
                            break;
                        case 8:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 56; h = 40; break; // CORRECT
                                case 1: w = 128; h = 32; break; // CORRECT
                                case 2: w = 92; h = 36; break; // CORRECT
                                case 3: w = 76; h = 36; break; // CORRECT
                            }
                            break;
                        case 9:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 56; h = 40; break; // CORRECT
                                case 1: w = 48; h = 56; break; // CORRECT
                                case 2: w = 56; h = 48; break; // CORRECT
                                case 3: w = 56; h = 44; break; // CORRECT
                            }
                            break;
                        case 10:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 48; h = 32; break; // CORRECT
                                case 1: w = 36; h = 28; break; // CORRECT
                                case 2: // CORRECT
                                case 3: w = 44; h = 32; break; // CORRECT
                            }
                            break;
                    }
                    break;
                case "COLONIST": // COMPLETE
                    switch (SelectedIndex)
                    {
                        case 0: // CORRECT
                        case 1: w = 32; h = 56; break; // CORRECT
                        case 2: w = 28; h = 60; break; // CORRECT
                        case 3: w = 40; h = 56; break; // CORRECT
                        case 4: w = 28; h = 60; break; // CORRECT
                        case 5: // CORRECT
                        case 6: // CORRECT
                        case 12: // CORRECT
                        case 13: w = 32; h = 60; break; // CORRECT
                        case 7: // CORRECT
                        case 8: // CORRECT
                        case 10: // CORRECT
                        case 14: w = 24; h = 60; break; // CORRECT
                        case 9: // CORRECT
                        case 11: w = 40; h = 60; break; // CORRECT
                        case 15:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 1: // CORRECT
                                case 2: w = 40; h = 60; break; // CORRECT
                            }
                            break;
                        case 16:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 36; h = 60; break; // CORRECT
                                case 1: w = 40; h = 64; break; // CORRECT
                                case 2: w = 36; h = 64; break; // CORRECT
                                case 3: w = 36; h = 44; break; // CORRECT
                            }
                            break;
                        case 17:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 40; h = 60; break; // CORRECT
                                case 1: w = 36; h = 60; break; // CORRECT
                                case 2: w = 44; h = 60; break; // CORRECT
                                case 3: w = 40; h = 56; break; // CORRECT
                            }
                            break;
                        case 19: w = 36; h = 44; break; // CORRECT
                        case 20: // CORRECT
                        case 21: w = 32; h = 44; break; // CORRECT
                        case 18: // CORRECT
                        case 22: w = 24; h = 44; break; // CORRECT
                    }
                    break;
                case "DOG": // COMPLETE
                    switch (SelectedIndex)
                    {
                        case 0:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 88; h = 104; break; // CORRECT
                                case 1: w = 68; h = 96; break; // CORRECT
                                case 2: w = 68; h = 92; break; // CORRECT
                                case 3: w = 80; h = 96; break; // CORRECT
                            }
                            break;
                        case 1:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 44; h = 104; break; // CORRECT
                                case 1: w = 48; h = 104; break; // CORRECT
                                case 2: w = 56; h = 104; break; // CORRECT
                                case 3: w = 68; h = 100; break; // CORRECT
                            }
                            break;
                        case 2:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 84; h = 100; break; // CORRECT
                                case 1: w = 80; h = 120; break; // CORRECT
                                case 2: w = 80; h = 92; break; // CORRECT
                                case 3: w = 84; h = 72; break; // CORRECT
                                case 4: w = 100; h = 60; break; // CORRECT
                                case 5: w = 108; h = 32; break; // CORRECT
                            }
                            break;
                        case 3:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 64; h = 92; break; // CORRECT
                                case 1: w = 60; h = 92; break; // CORRECT
                                case 2: w = 96; h = 104; break; // CORRECT
                                case 3: w = 112; h = 96; break; // CORRECT
                                case 4: w = 108; h = 76; break; // CORRECT
                                case 5: w = 116; h = 32; break; // CORRECT
                            }
                            break;
                        case 4:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 56; h = 80; break; // CORRECT
                                case 1: w = 56; h = 84; break; // CORRECT
                                case 2: w = 56; h = 88; break; // CORRECT
                                case 3: w = 68; h = 88; break; // CORRECT
                                case 4: // CORRECT
                                case 6: w = 72; h = 84; break; // CORRECT
                                case 5: w = 68; h = 84; break; // CORRECT
                                case 7: w = 72; h = 96; break; // CORRECT
                            }
                            break;
                        case 5:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 80; h = 80; break; // CORRECT
                                case 1: w = 76; h = 84; break; // CORRECT
                                case 2: w = 88; h = 92; break; // CORRECT
                                case 3: w = 108; h = 88; break; // CORRECT
                                case 4: w = 112; h = 84; break; // CORRECT
                                case 5: w = 100; h = 84; break; // CORRECT
                                case 6: w = 96; h = 84; break; // CORRECT
                                case 7: w = 72; h = 92; break; // CORRECT
                            }
                            break;
                        case 6:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 5: w = 100; h = 84; break; // CORRECT
                                case 1: // CORRECT
                                case 7: w = 100; h = 88; break; // CORRECT
                                case 2: // CORRECT
                                case 3: w = 104; h = 88; break; // CORRECT
                                case 4: w = 108; h = 84; break; // CORRECT
                                case 6: w = 96; h = 84; break; // CORRECT
                            }
                            break;
                        case 7:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 104; h = 88; break; // CORRECT
                                case 1: // CORRECT
                                case 4: w = 84; h = 88; break; // CORRECT
                                case 2: w = 72; h = 88; break; // CORRECT
                                case 3: w = 68; h = 88; break; // CORRECT
                                case 5: w = 76; h = 84; break; // CORRECT
                                case 6: w = 72; h = 84; break; // CORRECT
                                case 7: w = 88; h = 84; break; // CORRECT
                            }
                            break;
                        case 8:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 1: // CORRECT
                                case 2: w = 60; h = 88; break; // CORRECT
                                case 3: // CORRECT
                                case 5: w = 68; h = 88; break; // CORRECT
                                case 4: w = 68; h = 92; break; // CORRECT
                                case 6: w = 76; h = 88; break; // CORRECT
                                case 7: w = 80; h = 88; break; // CORRECT
                            }
                            break;
                        case 9:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 60; h = 92; break; // CORRECT
                                case 1: w = 64; h = 100; break; // CORRECT
                                case 2: w = 48; h = 108; break; // CORRECT
                                case 3: w = 56; h = 112; break; // CORRECT
                                case 4: w = 56; h = 116; break; // CORRECT
                                case 5: w = 72; h = 112; break; // CORRECT
                                case 6: w = 64; h = 92; break; // CORRECT
                                case 7: w = 60; h = 96; break; // CORRECT
                            }
                            break;
                        case 10:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 60; h = 92; break; // CORRECT
                                case 1: w = 64; h = 100; break; // CORRECT
                                case 2: w = 52; h = 112; break; // CORRECT
                                case 3: w = 60; h = 108; break; // CORRECT
                                case 4: w = 68; h = 112; break; // CORRECT
                                case 5: w = 96; h = 100; break; // CORRECT
                                case 6: w = 108; h = 92; break; // CORRECT
                                case 7: w = 60; h = 100; break; // CORRECT
                            }
                            break;
                    }
                    break;
                case "DOGCEIL": // COMPLETE
                    switch (SelectedIndex)
                    {
                        case 0:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 3: // CORRECT
                                case 4: // CORRECT
                                case 6: w = 60; h = 60; break; // CORRECT
                                case 1: w = 64; h = 56; break; // CORRECT
                                case 2: w = 64; h = 64; break; // CORRECT
                                case 5: w = 60; h = 56; break; // CORRECT
                                case 7: w = 60; h = 84; break; // CORRECT
                            }
                            break;
                        case 1:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 56; h = 56; break; // CORRECT
                                case 1: w = 52; h = 56; break; // CORRECT
                                case 2: w = 52; h = 56; break; // CORRECT
                                case 3: w = 80; h = 60; break; // CORRECT
                                case 4: w = 96; h = 60; break; // CORRECT
                                case 5: w = 108; h = 56; break; // CORRECT
                                case 6: w = 104; h = 60; break; // CORRECT
                                case 7: w = 88; h = 80; break; // CORRECT
                            }
                            break;
                        case 2:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 80; h = 113; break; // CORRECT
                                case 1: w = 72; h = 96; break; // CORRECT
                                case 2: w = 64; h = 88; break; // CORRECT
                            }
                            break;
                        case 3:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 60; h = 88; break; // CORRECT
                                case 1: w = 64; h = 84; break; // CORRECT
                                case 2: w = 72; h = 80; break; // CORRECT
                                case 3: w = 68; h = 96; break; // CORRECT
                                case 4: w = 56; h = 116; break; // CORRECT
                                case 5: w = 72; h = 100; break; // CORRECT
                            }
                            break;
                        case 4:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 92; h = 77; break; // CORRECT
                                case 1: w = 84; h = 124; break; // CORRECT
                                case 2: w = 64; h = 92; break; // CORRECT
                                case 3: w = 80; h = 52; break; // CORRECT
                                case 4: w = 92; h = 60; break; // CORRECT
                                case 5: w = 108; h = 32; break; // CORRECT
                            }
                            break;
                    }
                    break;
                case "EGGS": // COMPLETE
                    switch (SelectedIndex)
                    {
                        case 0:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 2: w = 48; h = 60; break; // CORRECT
                                case 1: w = 48; h = 56; break; // CORRECT
                            }
                            break;
                        case 1: // COMPLETE
                            w = 48; h = 60; // CORRECT
                            break;
                        case 2:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 48; h = 64; break; // CORRECT
                                case 1: w = 52; h = 64; break; // CORRECT
                                case 2: w = 52; h = 60; break; // CORRECT
                                case 3: w = 52; h = 44; break; // CORRECT
                                case 4: w = 52; h = 32; break; // CORRECT
                            }
                            break;
                        case 3: // COMPLETE
                            w = 52; h = 32; // CORRECT
                            break;
                        case 4:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 1: // CORRECT
                                case 2: // CORRECT
                                case 3: // CORRECT
                                case 4: // CORRECT
                                case 5: // CORRECT
                                case 6: w = 48; h = 64; break; // CORRECT
                                case 7: w = 48; h = 60; break; // CORRECT
                            }
                            break;
                        case 5:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 1: // CORRECT
                                case 2: // CORRECT
                                case 4: // CORRECT
                                case 5: // CORRECT
                                case 6: // CORRECT
                                case 7: w = 48; h = 60; break; // CORRECT
                                case 3: w = 48; h = 64; break; // CORRECT
                            }
                            break;
                    }
                    break;
                case "FINGERS": // COMPLETE
                    switch (SelectedIndex)
                    {
                        case 0:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 60; h = 88; break; // CORRECT
                                case 1: w = 64; h = 88; break; // CORRECT
                                case 2: w = 56; h = 88; break; // CORRECT
                                case 3: w = 52; h = 92; break; // CORRECT
                            }
                            break;
                        case 1:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 60; h = 92; break; // CORRECT
                                case 1: w = 52; h = 88; break; // CORRECT
                                case 2: w = 56; h = 88; break; // CORRECT
                                case 3: w = 64; h = 88; break; // CORRECT
                            }
                            break;
                    }
                    break;
                case "GUARD": // COMPLETE
                    switch (SelectedIndex)
                    {
                        case 0:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 56; h = 120; break; // CORRECT
                                case 1: // CORRECT
                                case 2: w = 64; h = 120; break; // CORRECT
                                case 3: w = 72; h = 112; break; // CORRECT
                            }
                            break;
                        case 1:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 44; h = 116; break; // CORRECT
                                case 1: w = 72; h = 120; break; // CORRECT
                                case 2: w = 64; h = 100; break; // CORRECT
                                case 3: w = 84; h = 76; break; // CORRECT
                                case 4: w = 88; h = 60; break; // CORRECT
                                case 5: w = 92; h = 32; break; // CORRECT
                            }
                            break;
                        case 2:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 32; h = 116; break; // CORRECT
                                case 1: w = 36; h = 120; break; // CORRECT
                                case 2: w = 36; h = 116; break; // CORRECT
                            }
                            break;
                        case 3:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 5: w = 56; h = 120; break; // CORRECT
                                case 1: // CORRECT
                                case 2: // CORRECT
                                case 4: w = 60; h = 120; break; // CORRECT
                                case 3: w = 60; h = 116; break; // CORRECT
                                case 6: // CORRECT
                                case 7: w = 52; h = 116; break; // CORRECT
                            }
                            break;
                        case 4:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 56; h = 116; break; // CORRECT
                                case 1: // CORRECT
                                case 2: // CORRECT
                                case 3: // CORRECT
                                case 4: w = 60; h = 116; break; // CORRECT
                                case 5: // CORRECT
                                case 6: // CORRECT
                                case 7: w = 52; h = 116; break; // CORRECT
                            }
                            break;
                        case 5:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 56; h = 116; break; // CORRECT
                                case 1: w = 28; h = 120; break; // CORRECT
                                case 2: w = 64; h = 120; break; // CORRECT
                                case 3: w = 72; h = 120; break; // CORRECT
                                case 4: w = 48; h = 116; break; // CORRECT
                                case 5: w = 36; h = 116; break; // CORRECT
                                case 6: w = 68; h = 116; break; // CORRECT
                                case 7: w = 72; h = 116; break; // CORRECT
                            }
                            break;
                        case 6:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 64; h = 120; break; // CORRECT
                                case 1: w = 44; h = 120; break; // CORRECT
                                case 2: w = 60; h = 120; break; // CORRECT
                                case 3: w = 68; h = 116; break; // CORRECT
                                case 4: w = 48; h = 116; break; // CORRECT
                                case 5: w = 56; h = 120; break; // CORRECT
                                case 6: // CORRECT
                                case 7: w = 72; h = 116; break; // CORRECT
                            }
                            break;
                        case 7:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 4: w = 56; h = 116; break; // CORRECT
                                case 1: w = 52; h = 116; break; // CORRECT
                                case 2: w = 72; h = 116; break; // CORRECT
                                case 3: w = 76; h = 120; break; // CORRECT
                                case 5: w = 44; h = 116; break; // CORRECT
                                case 6: w = 64; h = 116; break; // CORRECT
                                case 7: w = 68; h = 116; break; // CORRECT
                            }
                            break;
                    }
                    break;
                case "HANDLER": // COMPLETE
                    switch (SelectedIndex)
                    {
                        case 0:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 7: w = 64; h = 104; break; // CORRECT
                                case 1: w = 52; h = 104; break; // CORRECT
                                case 2: w = 56; h = 104; break; // CORRECT
                                case 3: // CORRECT
                                case 6: w = 68; h = 104; break; // CORRECT
                                case 4: w = 56; h = 108; break; // CORRECT
                                case 5: w = 48; h = 108; break; // CORRECT
                            }
                            break;
                        case 1:
                            h = 104;
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 1: // CORRECT
                                case 4: // CORRECT
                                case 7: w = 52; break; // CORRECT
                                case 2: // CORRECT
                                case 3: w = 48; break; // CORRECT
                                case 5: // CORRECT
                                case 6: w = 56; break; // CORRECT
                            }
                            break;
                        case 2:
                            h = 104;
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 5: // CORRECT
                                case 6: // CORRECT
                                case 7: w = 48; break; // CORRECT
                                case 1: w = 44; break; // CORRECT
                                case 2: // CORRECT
                                case 4: w = 52; break; // CORRECT
                                case 3: w = 60; break; // CORRECT
                            }
                            break;
                        case 3:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 56; h = 104; break; // CORRECT
                                case 1: w = 56; h = 104; break; // CORRECT
                                case 2: w = 52; h = 104; break; // CORRECT
                                case 3: w = 52; h = 104; break; // CORRECT
                                case 4: w = 56; h = 104; break; // CORRECT
                                case 5: w = 60; h = 108; break; // CORRECT
                                case 6: w = 60; h = 104; break; // CORRECT
                                case 7: w = 56; h = 104; break; // CORRECT
                            }
                            break;
                        case 4:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 68; h = 104; break; // CORRECT
                                case 1: w = 60; h = 108; break; // CORRECT
                                case 2: w = 60; h = 104; break; // CORRECT
                                case 3: w = 64; h = 104; break; // CORRECT
                                case 4: w = 64; h = 108; break; // CORRECT
                                case 5: w = 60; h = 108; break; // CORRECT
                                case 6: w = 68; h = 104; break; // CORRECT
                                case 7: w = 72; h = 104; break; // CORRECT
                            }
                            break;
                        case 5:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 56; h = 108; break; // CORRECT
                                case 1: w = 108; h = 108; break; // CORRECT
                                case 2: w = 76; h = 100; break; // CORRECT
                                case 3: w = 92; h = 112; break; // CORRECT
                            }
                            break;
                        case 6:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 68; h = 104; break; // CORRECT
                                case 1: w = 88; h = 124; break; // CORRECT
                                case 2: w = 88; h = 120; break; // CORRECT
                                case 3: w = 108; h = 116; break; // CORRECT
                                case 4: w = 108; h = 88; break; // CORRECT
                                case 5: w = 112; h = 32; break; // CORRECT
                            }
                            break;
                        case 7:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 48; h = 104; break; // CORRECT
                                case 1: w = 48; h = 112; break; // CORRECT
                                case 2: w = 44; h = 108; break; // CORRECT
                                case 3: w = 48; h = 104; break; // CORRECT
                                case 4: w = 44; h = 104; break; // CORRECT
                                case 5: w = 56; h = 100; break; // CORRECT
                                case 6: w = 48; h = 100; break; // CORRECT
                                case 7: w = 48; h = 104; break; // CORRECT
                            }
                            break;
                        case 8:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 44; h = 100; break; // CORRECT
                                case 1: // CORRECT
                                case 2: w = 44; h = 104; break; // CORRECT
                            }
                            break;
                    }
                    break;
                case "HUGGER": // COMPLETE
                    switch (SelectedIndex)
                    {
                        case 0:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 44; h = 40; break; // CORRECT
                                case 1: w = 48; h = 40; break; // CORRECT
                                case 2: // CORRECT
                                case 3: w = 48; h = 36; break; // CORRECT
                            }
                            break;
                        case 1:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 92; h = 32; break; // CORRECT
                                case 1: w = 92; h = 32; break; // CORRECT
                                case 2: w = 108; h = 32; break; // CORRECT
                                case 3: w = 108; h = 28; break; // CORRECT
                            }
                            break;
                        case 2:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 1: w = 124; h = 28; break; // CORRECT
                                case 2: w = 112; h = 32; break; // CORRECT
                                case 3: w = 120; h = 32; break; // CORRECT
                            }
                            break;
                        case 3:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 92; h = 32; break; // CORRECT
                                case 1: w = 96; h = 28; break; // CORRECT
                                case 2: w = 76; h = 40; break; // CORRECT
                                case 3: w = 80; h = 36; break; // CORRECT
                            }
                            break;
                        case 4:
                            w = 48;
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 3: h = 40; break; // CORRECT
                                case 1: h = 36; break; // CORRECT
                                case 2: h = 44; break; // CORRECT
                            }
                            break;
                        case 5:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 64; h = 32; break; // CORRECT
                                case 1: w = 96; h = 56; break; // CORRECT
                                case 2: w = 112; h = 52; break; // CORRECT
                                case 3: w = 84; h = 40; break; // CORRECT
                                case 4: w = 48; h = 64; break; // CORRECT
                                case 5: w = 52; h = 64; break; // CORRECT
                                case 6: w = 48; h = 52; break; // CORRECT
                            }
                            break;
                        case 6:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 76; h = 40; break; // CORRECT
                                case 1: w = 88; h = 56; break; // CORRECT
                                case 2: w = 112; h = 56; break; // CORRECT
                                case 3: w = 124; h = 64; break; // CORRECT
                                case 4: w = 120; h = 56; break; // CORRECT
                                case 5: w = 108; h = 32; break; // CORRECT
                            }
                            break;
                        case 7:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 1: // CORRECT
                                case 2: // CORRECT
                                case 3: // CORRECT
                                case 4: w = 48; h = 64; break; // CORRECT
                                case 5: w = 52; h = 64; break; // CORRECT
                                case 6: w = 48; h = 52; break; // CORRECT
                                case 7: w = 48; h = 56; break; // CORRECT
                            }
                            break;
                    }
                    break;
                case "QUEEN": // COMPLETE
                    switch (SelectedIndex)
                    {
                        case 0:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 148; h = 168; break; // CORRECT
                                case 1: w = 144; h = 172; break; // CORRECT
                                case 2: w = 140; h = 176; break; // CORRECT
                                case 3: w = 144; h = 180; break; // CORRECT
                            }
                            break;
                        case 1:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 124; h = 180; break; // CORRECT
                                case 1: w = 128; h = 172; break; // CORRECT
                                case 2: w = 128; h = 168; break; // CORRECT
                                case 3: w = 128; h = 164; break; // CORRECT
                                case 4: w = 132; h = 168; break; // CORRECT
                            }
                            break;
                        case 2:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 132; h = 156; break; // CORRECT
                                case 1: w = 140; h = 156; break; // CORRECT
                                case 2: w = 140; h = 152; break; // CORRECT
                                case 3: w = 132; h = 152; break; // CORRECT
                                case 4: w = 140; h = 160; break; // CORRECT
                                case 5: w = 184; h = 156; break; // CORRECT
                                case 6: w = 180; h = 160; break; // CORRECT
                                case 7: w = 144; h = 172; break; // CORRECT
                                case 8: w = 144; h = 184; break; // CORRECT
                                case 9: w = 112; h = 176; break; // CORRECT
                            }
                            break;
                        case 3:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 148; h = 204; break; // CORRECT
                                case 1: w = 168; h = 212; break; // CORRECT
                                case 2: w = 212; h = 240; break; // CORRECT
                                case 3: w = 252; h = 232; break; // CORRECT
                                case 4: w = 256; h = 200; break; // CORRECT
                                case 5: w = 184; h = 212; break; // CORRECT
                            }
                            break;
                        case 4:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 208; h = 192; break; // CORRECT
                                case 1: w = 192; h = 192; break; // CORRECT
                                case 2: w = 180; h = 188; break; // CORRECT
                                case 3: w = 160; h = 188; break; // CORRECT
                                case 4: w = 128; h = 188; break; // CORRECT
                                case 5: w = 124; h = 180; break; // CORRECT
                                case 6: w = 132; h = 172; break; // CORRECT
                                case 7: w = 144; h = 176; break; // CORRECT
                                case 8: w = 148; h = 172; break; // CORRECT
                                case 9: w = 184; h = 176; break; // CORRECT
                                case 10: w = 212; h = 176; break; // CORRECT
                            }
                            break;
                        case 5:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 128; h = 180; break; // CORRECT
                                case 1: w = 124; h = 176; break; // CORRECT
                                case 2: w = 120; h = 172; break; // CORRECT
                                case 3: w = 120; h = 168; break; // CORRECT
                                case 4: w = 120; h = 172; break; // CORRECT
                                case 5: w = 140; h = 172; break; // CORRECT
                                case 6: w = 148; h = 176; break; // CORRECT
                                case 7: w = 140; h = 180; break; // CORRECT
                                case 8: w = 128; h = 180; break; // CORRECT
                                case 9: w = 128; h = 176; break; // CORRECT
                                case 10: w = 136; h = 176; break; // CORRECT
                            }
                            break;
                        case 6:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 168; h = 188; break; // CORRECT
                                case 1: w = 188; h = 208; break; // CORRECT
                                case 2: w = 212; h = 224; break; // CORRECT
                                case 3: w = 156; h = 200; break; // CORRECT
                                case 4: w = 144; h = 184; break; // CORRECT
                            }
                            break;
                        case 7:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 148; h = 148; break; // CORRECT
                                case 1: w = 148; h = 128; break; // CORRECT
                                case 2: w = 160; h = 120; break; // CORRECT
                                case 3: w = 236; h = 100; break; // CORRECT
                                case 4: w = 248; h = 80; break; // CORRECT
                                case 5: w = 256; h = 76; break; // CORRECT
                            }
                            break;
                        case 8:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 136; h = 184; break; // CORRECT
                                case 1: w = 164; h = 172; break; // CORRECT
                                case 2: w = 200; h = 180; break; // CORRECT
                                case 3: w = 204; h = 188; break; // CORRECT
                                case 4: w = 232; h = 188; break; // CORRECT
                                case 5: w = 152; h = 176; break; // CORRECT
                            }
                            break;
                        case 9:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 152; h = 184; break; // CORRECT
                                case 1: w = 164; h = 184; break; // CORRECT
                                case 2: w = 172; h = 188; break; // CORRECT
                            }
                            break;
                        case 10:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 128; h = 176; break; // CORRECT
                                case 1: w = 132; h = 184; break; // CORRECT
                                case 2: w = 120; h = 188; break; // CORRECT
                                case 3: w = 140; h = 188; break; // CORRECT
                            }
                            break;
                        case 11:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 140; h = 192; break; // CORRECT
                                case 1: w = 148; h = 208; break; // CORRECT
                                case 2: w = 200; h = 236; break; // CORRECT
                                case 3: w = 196; h = 212; break; // CORRECT
                                case 4: w = 208; h = 208; break; // CORRECT
                                case 5: w = 200; h = 204; break; // CORRECT
                                case 6: w = 188; h = 200; break; // CORRECT
                            }
                            break;
                    }
                    break;
                case "SOLDIER": // COMPLETE
                    switch (SelectedIndex)
                    {
                        case 0:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 48; h = 112; break; // CORRECT
                                case 1: // CORRECT
                                case 2: w = 96; h = 112; break; // CORRECT
                                case 3: w = 100; h = 108; break; // CORRECT
                            }
                            break;
                        case 1:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 44; h = 108; break; // CORRECT
                                case 1: w = 44; h = 116; break; // CORRECT
                                case 2: w = 40; h = 116; break; // CORRECT
                            }
                            break;
                        case 2:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 48; h = 108; break; // CORRECT
                                case 1: w = 60; h = 108; break; // CORRECT
                                case 2: w = 56; h = 108; break; // CORRECT
                                case 3: w = 80; h = 92; break; // CORRECT
                                case 4: w = 80; h = 68; break; // CORRECT
                                case 5: w = 96; h = 32; break; // CORRECT
                            }
                            break;
                        case 3:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 60; h = 116; break; // CORRECT
                                case 1: w = 60; h = 112; break; // CORRECT
                                case 2: w = 52; h = 112; break; // CORRECT
                                case 3: w = 44; h = 116; break; // CORRECT
                                case 4: w = 40; h = 116; break; // CORRECT
                                case 5: w = 40; h = 112; break; // CORRECT
                                case 6: w = 48; h = 112; break; // CORRECT
                                case 7: w = 52; h = 116; break; // CORRECT
                            }
                            break;
                        case 4:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 60; h = 112; break; // CORRECT
                                case 1: w = 68; h = 108; break; // CORRECT
                                case 2: w = 64; h = 108; break; // CORRECT
                                case 3: w = 52; h = 112; break; // CORRECT
                                case 4: w = 68; h = 112; break; // CORRECT
                                case 5: w = 76; h = 108; break; // CORRECT
                                case 6: w = 60; h = 112; break; // CORRECT
                                case 7: w = 52; h = 112; break; // CORRECT
                            }
                            break;
                        case 5:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 60; h = 108; break; // CORRECT
                                case 1: w = 60; h = 112; break; // CORRECT
                                case 2: w = 56; h = 112; break; // CORRECT
                                case 3: w = 48; h = 112; break; // CORRECT
                                case 4: w = 44; h = 112; break; // CORRECT
                                case 5: w = 44; h = 108; break; // CORRECT
                                case 6: w = 52; h = 112; break; // CORRECT
                                case 7: w = 56; h = 112; break; // CORRECT
                            }
                            break;
                        case 6:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 40; h = 116; break; // CORRECT
                                case 1: w = 48; h = 112; break; // CORRECT
                                case 2: w = 44; h = 112; break; // CORRECT
                                case 3: w = 36; h = 112; break; // CORRECT
                                case 4: w = 56; h = 112; break; // CORRECT
                                case 5: w = 60; h = 112; break; // CORRECT
                                case 6: w = 44; h = 112; break; // CORRECT
                                case 7: w = 36; h = 116; break; // CORRECT
                            }
                            break;
                        case 7:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 68; h = 112; break; // CORRECT
                                case 1: w = 76; h = 108; break; // CORRECT
                                case 2: w = 72; h = 108; break; // CORRECT
                                case 3: // CORRECT
                                case 6: // CORRECT
                                case 7: w = 64; h = 112; break; // CORRECT
                                case 4: w = 60; h = 108; break; // CORRECT
                                case 5: w = 64; h = 108; break; // CORRECT
                            }
                            break;
                    }
                    break;
                case "SYNTH": // COMPLETE
                    switch (SelectedIndex)
                    {
                        case 0:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 84; h = 104; break; // CORRECT
                                case 1: w = 68; h = 96; break; // CORRECT
                                case 2: w = 80; h = 104; break; // CORRECT
                                case 3: w = 100; h = 112; break; // CORRECT
                            }
                            break;
                        case 1:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 44; h = 112; break; // CORRECT
                                case 1: w = 68; h = 108; break; // CORRECT
                                case 2: w = 96; h = 108; break; // CORRECT
                            }
                            break;
                        case 2:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 72; h = 120; break; // CORRECT
                                case 1: w = 88; h = 100; break; // CORRECT
                                case 2: w = 108; h = 108; break; // CORRECT
                                case 3: w = 128; h = 84; break; // CORRECT
                                case 4: w = 120; h = 28; break; // CORRECT
                            }
                            break;
                        case 3:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 88; h = 128; break; // CORRECT
                                case 1: w = 76; h = 128; break; // CORRECT
                                case 2: w = 92; h = 100; break; // CORRECT
                                case 3: w = 108; h = 72; break; // CORRECT
                                case 4: w = 108; h = 28; break; // CORRECT
                            }
                            break;
                        case 4:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 40; h = 112; break; // CORRECT
                                case 1: w = 64; h = 112; break; // CORRECT
                                case 2: w = 44; h = 112; break; // CORRECT
                            }
                            break;
                        case 5:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 3: w = 72; h = 120; break; // CORRECT
                                case 1: // CORRECT
                                case 2: // CORRECT
                                case 7: w = 72; h = 124; break; // CORRECT
                                case 4: // CORRECT
                                case 5: // CORRECT
                                case 6: w = 68; h = 124; break; // CORRECT
                            }
                            break;
                        case 6:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 1: // CORRECT
                                case 2: // CORRECT
                                case 3: w = 68; h = 116; break; // CORRECT
                                case 4: // CORRECT
                                case 5: // CORRECT
                                case 6: // CORRECT
                                case 7: w = 64; h = 120; break; // CORRECT
                            }
                            break;
                        case 7:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 68; h = 116; break; // CORRECT
                                case 1: w = 52; h = 116; break; // CORRECT
                                case 2: w = 44; h = 116; break; // CORRECT
                                case 3: w = 60; h = 116; break; // CORRECT
                                case 4: w = 68; h = 120; break; // CORRECT
                                case 5: w = 60; h = 120; break; // CORRECT
                                case 6: w = 52; h = 120; break; // CORRECT
                                case 7: w = 64; h = 120; break; // CORRECT
                            }
                            break;
                        case 8:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 64; h = 120; break; // CORRECT
                                case 1: // CORRECT
                                case 6: w = 52; h = 124; break; // CORRECT
                                case 2: w = 48; h = 124; break; // CORRECT
                                case 3: // CORRECT
                                case 7: w = 60; h = 124; break; // CORRECT
                                case 4: w = 64; h = 124; break; // CORRECT
                                case 5: w = 56; h = 124; break; // CORRECT
                            }
                            break;
                        case 9:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 72; h = 120; break; // CORRECT
                                case 1: w = 68; h = 120; break; // CORRECT
                                case 2: // CORRECT
                                case 3: // CORRECT
                                case 4: w = 60; h = 120; break; // CORRECT
                                case 5: w = 56; h = 120; break; // CORRECT
                                case 6: w = 60; h = 124; break; // CORRECT
                                case 7: w = 64; h = 120; break; // CORRECT
                            }
                            break;
                        case 10:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 7: w = 64; h = 124; break; // CORRECT
                                case 1: // CORRECT
                                case 2: // CORRECT
                                case 6: w = 60; h = 124; break; // CORRECT
                                case 3: // CORRECT
                                case 5: w = 68; h = 124; break; // CORRECT
                                case 4: w = 76; h = 124; break; // CORRECT
                            }
                            break;
                        case 11:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 64; h = 116; break; // CORRECT
                                case 1: // CORRECT
                                case 2: w = 60; h = 116; break; // CORRECT
                                case 3: w = 72; h = 116; break; // CORRECT
                                case 4: w = 76; h = 116; break; // CORRECT
                                case 5: w = 68; h = 120; break; // CORRECT
                                case 6: // CORRECT
                                case 7: w = 60; h = 120; break; // CORRECT
                            }
                            break;
                        case 12:
                            h = 120;
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 72; break; // CORRECT
                                case 1: // CORRECT
                                case 7: w = 68; break; // CORRECT
                                case 2: // CORRECT
                                case 3: // CORRECT
                                case 4: w = 60; break; // CORRECT
                                case 5: // CORRECT
                                case 6: w = 56; break; // CORRECT
                            }
                            break;
                        case 13:
                            h = 108;
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 2: w = 40; break; // CORRECT
                                case 1: w = 52; break; // CORRECT
                            }
                            break;
                        case 14:
                            h = 108;
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 100; break; // CORRECT
                                case 1: w = 124; break; // CORRECT
                                case 2: w = 96; break; // CORRECT
                            }
                            break;
                        case 15:
                            h = 108;
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 96; break; // CORRECT
                                case 1: w = 120; break; // CORRECT
                                case 2: w = 92; break; // CORRECT
                            }
                            break;
                        case 16:
                            h = 108;
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 2: w = 68; break; // CORRECT
                                case 1: w = 104; break; // CORRECT
                            }
                            break;
                        case 17:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 84; h = 108; break; // CORRECT
                                case 1: w = 88; h = 112; break; // CORRECT
                                case 2: w = 80; h = 108; break; // CORRECT
                            }
                            break;
                        case 18:
                            h = 108;
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 80; break; // CORRECT
                                case 1: w = 92; break; // CORRECT
                                case 2: w = 76; break; // CORRECT
                            }
                            break;
                        case 19:
                            h = 108;
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 64; break; // CORRECT
                                case 1: w = 96; break; // CORRECT
                                case 2: w = 68; break; // CORRECT
                            }
                            break;
                    }
                    break;
                case "WAR": // COMPLETE
                    switch (SelectedIndex)
                    {
                        case 0:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 76; h = 92; break; // CORRECT
                                case 1: w = 72; h = 92; break; // CORRECT
                                case 2: w = 92; h = 96; break; // CORRECT
                            }
                            break;
                        case 1:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 48; h = 100; break; // CORRECT
                                case 1: w = 60; h = 104; break; // CORRECT
                                case 2: w = 80; h = 100; break; // CORRECT
                            }
                            break;
                        case 2:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 60; h = 96; break; // CORRECT
                                case 1: w = 80; h = 100; break; // CORRECT
                                case 2: w = 108; h = 96; break; // CORRECT
                                case 3: w = 68; h = 88; break; // CORRECT
                                case 4: w = 92; h = 72; break; // CORRECT
                                case 5: w = 116; h = 32; break; // CORRECT
                            }
                            break;
                        case 3:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 1: // CORRECT
                                case 7: w = 56; h = 84; break; // CORRECT
                                case 2: // CORRECT
                                case 5: w = 44; h = 88; break; // CORRECT
                                case 3: // CORRECT
                                case 4: w = 40; h = 84; break; // CORRECT
                                case 6: w = 52; h = 88; break; // CORRECT
                            }
                            break;
                        case 4:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 88; h = 84; break; // CORRECT
                                case 1: // CORRECT
                                case 7: w = 84; h = 84; break; // CORRECT
                                case 2: w = 80; h = 84; break; // CORRECT
                                case 3: // CORRECT
                                case 4: w = 76; h = 84; break; // CORRECT
                                case 5: // CORRECT
                                case 6: w = 76; h = 88; break; // CORRECT
                            }
                            break;
                        case 5:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 112; h = 80; break; // CORRECT
                                case 1: w = 108; h = 84; break; // CORRECT
                                case 2: // CORRECT
                                case 5: w = 108; h = 88; break; // CORRECT
                                case 3: // CORRECT
                                case 4: w = 104; h = 88; break; // CORRECT
                                case 6: // CORRECT
                                case 7: w = 104; h = 84; break; // CORRECT
                            }
                            break;
                        case 6:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 7: w = 76; h = 84; break; // CORRECT
                                case 1: w = 88; h = 84; break; // CORRECT
                                case 2: w = 92; h = 92; break; // CORRECT
                                case 3: // CORRECT
                                case 5: w = 84; h = 88; break; // CORRECT
                                case 4: // CORRECT
                                case 6: w = 80; h = 88; break; // CORRECT
                            }
                            break;
                        case 7:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: // CORRECT
                                case 1: w = 60; h = 84; break; // CORRECT
                                case 2: w = 52; h = 92; break; // CORRECT
                                case 3: // CORRECT
                                case 4: w = 48; h = 88; break; // CORRECT
                                case 5: w = 48; h = 84; break; // CORRECT
                                case 6: w = 56; h = 92; break; // CORRECT
                                case 7: w = 56; h = 88; break; // CORRECT
                            }
                            break;
                        case 8:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 52; h = 88; break; // CORRECT
                                case 1: w = 64; h = 92; break; // CORRECT
                                case 2: w = 68; h = 92; break; // CORRECT
                                case 3: w = 56; h = 116; break; // CORRECT
                                case 4: w = 68; h = 120; break; // CORRECT
                            }
                            break;
                        case 9:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 52; h = 96; break; // CORRECT
                                case 1: w = 56; h = 100; break; // CORRECT
                                case 2: w = 60; h = 112; break; // CORRECT
                                case 3: w = 64; h = 116; break; // CORRECT
                                case 4: w = 120; h = 124; break; // CORRECT
                                case 5: w = 60; h = 104; break; // CORRECT
                            }
                            break;
                    }
                    break;
                case "WARCEIL": // COMPLETE
                    switch (SelectedIndex)
                    {
                        case 0:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 112; h = 84; break; // CORRECT
                                case 1: w = 88; h = 120; break; // CORRECT
                                case 2: w = 80; h = 88; break; // CORRECT
                                case 3: w = 116; h = 68; break; // CORRECT
                                case 4: w = 120; h = 40; break; // CORRECT
                            }
                            break;
                        case 1:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 64; h = 96; break; // CORRECT
                                case 1: w = 72; h = 64; break; // CORRECT
                                case 2: w = 64; h = 68; break; // CORRECT
                            }
                            break;
                        case 2:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 64; h = 76; break; // CORRECT
                                case 1: w = 64; h = 80; break; // CORRECT
                                case 2: // CORRECT
                                case 6: w = 64; h = 84; break; // CORRECT
                                case 3: // CORRECT
                                case 4: w = 64; h = 64; break; // CORRECT
                                case 5: w = 68; h = 68; break; // CORRECT
                                case 7: w = 64; h = 92; break; // CORRECT
                            }
                            break;
                        case 3:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 60; h = 80; break; // CORRECT
                                case 1: w = 52; h = 80; break; // CORRECT
                                case 2: w = 56; h = 84; break; // CORRECT
                                case 3: w = 80; h = 64; break; // CORRECT
                                case 4: w = 84; h = 64; break; // CORRECT
                                case 5: w = 80; h = 68; break; // CORRECT
                                case 6: w = 72; h = 84; break; // CORRECT
                                case 7: w = 64; h = 92; break; // CORRECT
                            }
                            break;
                        case 4:
                            switch (FrameIndex) // COMPLETE
                            {
                                case 0: w = 68; h = 84; break; // CORRECT
                                case 1: w = 60; h = 96; break; // CORRECT
                                case 2: w = 56; h = 116; break; // CORRECT
                                case 3: w = 76; h = 100; break; // CORRECT
                            }
                            break;
                    }
                    break;
            }
            return (w, h);
        }
    }
}