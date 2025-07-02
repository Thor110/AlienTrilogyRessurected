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
                    case 0:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 32; h = 59; break; // CORRECT
                            case 1: w = 32; h = 59; break;
                            case 2: w = 32; h = 59; break;
                            case 3: w = 32; h = 59; break;
                            case 4: w = 32; h = 59; break;
                            case 5: w = 32; h = 59; break;
                            case 6: w = 32; h = 59; break;
                            case 7: w = 32; h = 59; break;
                        }
                        break;
                    case 1:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 40; h = 64; break; // CORRECT
                            case 1: w = 40; h = 64; break;
                            case 2: w = 40; h = 64; break;
                            case 3: w = 40; h = 64; break;
                            case 4: w = 40; h = 64; break;
                            case 5: w = 40; h = 64; break;
                        }
                        break;
                    case 2:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 44; h = 72; break; // CORRECT
                            case 1: w = 44; h = 72; break;
                            case 2: w = 44; h = 72; break;
                            case 3: w = 44; h = 72; break;
                            case 4: w = 44; h = 72; break;
                            case 5: w = 44; h = 72; break;
                        }
                        break;
                    case 3:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 92; h = 67; break; // CORRECT
                            case 1: w = 92; h = 67; break;
                            case 2: w = 92; h = 67; break;
                            case 3: w = 92; h = 67; break;
                            case 4: w = 92; h = 67; break;
                            case 5: w = 92; h = 67; break;
                            case 6: w = 92; h = 67; break;
                            case 7: w = 92; h = 67; break;
                        }
                        break;
                    case 4:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 32; h = 69; break; // CORRECT
                            case 1: w = 32; h = 69; break;
                            case 2: w = 32; h = 69; break;
                            case 3: w = 32; h = 69; break;
                            case 4: w = 32; h = 69; break;
                            case 5: w = 32; h = 69; break;
                            case 6: w = 32; h = 69; break;
                            case 7: w = 32; h = 69; break;
                        }
                        break;
                    case 5:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 32; h = 70; break; // CORRECT
                            case 1: w = 32; h = 70; break;
                            case 2: w = 32; h = 70; break;
                            case 3: w = 32; h = 70; break;
                            case 4: w = 32; h = 70; break;
                            case 5: w = 32; h = 70; break;
                            case 6: w = 32; h = 70; break;
                            case 7: w = 32; h = 70; break;
                        }
                        break;
                    case 6:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 64; h = 69; break; // CORRECT
                            case 1: w = 64; h = 69; break;
                            case 2: w = 64; h = 69; break;
                            case 3: w = 64; h = 69; break;
                            case 4: w = 64; h = 69; break;
                            case 5: w = 64; h = 69; break;
                            case 6: w = 64; h = 69; break;
                            case 7: w = 64; h = 69; break;
                        }
                        break;
                    case 7:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 64; h = 69; break; // CORRECT
                            case 1: w = 64; h = 69; break;
                            case 2: w = 64; h = 69; break;
                            case 3: w = 64; h = 69; break;
                            case 4: w = 64; h = 69; break;
                            case 5: w = 64; h = 69; break;
                            case 6: w = 64; h = 69; break;
                            case 7: w = 64; h = 69; break;
                        }
                        break;
                    case 8:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 44; h = 67; break; // CORRECT
                            case 1: w = 44; h = 67; break;
                            case 2: w = 44; h = 67; break;
                            case 3: w = 44; h = 67; break;
                            case 4: w = 44; h = 67; break;
                            case 5: w = 44; h = 67; break;
                        }
                        break;
                    case 9:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 96; h = 33; break; // CORRECT
                            case 1: w = 96; h = 33; break;
                            case 2: w = 96; h = 33; break;
                            case 3: w = 96; h = 33; break;
                            case 4: w = 96; h = 33; break;
                            case 5: w = 96; h = 33; break;
                        }
                        break;
                }
            }
            else if (lastSelectedFile == "BURSTER") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 40; h = 48; break; // CORRECT
                            case 1: w = 40; h = 48; break;
                            case 2: w = 40; h = 48; break;
                            case 3: w = 40; h = 48; break;
                        }
                        break;
                    case 1:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 32; h = 30; break; // CORRECT
                            case 1: w = 32; h = 30; break;
                            case 2: w = 32; h = 30; break;
                            case 3: w = 32; h = 30; break;
                            case 4: w = 32; h = 30; break;
                            case 5: w = 32; h = 30; break;
                            case 6: w = 32; h = 30; break;
                            case 7: w = 32; h = 30; break;
                        }
                        break;
                    case 2:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 52; h = 61; break; // CORRECT
                            case 1: w = 52; h = 61; break;
                            case 2: w = 52; h = 61; break;
                            case 3: w = 52; h = 61; break;
                        }
                        break;
                    case 3:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 36; h = 35; break; // CORRECT
                            case 1: w = 36; h = 35; break;
                            case 2: w = 36; h = 35; break;
                            case 3: w = 36; h = 35; break;
                        }
                        break;
                    case 4:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 84; h = 61; break; // CORRECT
                            case 1: w = 84; h = 61; break;
                            case 2: w = 84; h = 61; break;
                            case 3: w = 84; h = 61; break;
                            case 4: w = 84; h = 61; break;
                        }
                        break;
                    case 5:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 52; h = 40; break; // CORRECT
                            case 1: w = 52; h = 40; break;
                            case 2: w = 52; h = 40; break;
                            case 3: w = 52; h = 40; break;
                            case 4: w = 52; h = 40; break;
                        }
                        break;
                    case 6:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 60; h = 42; break; // CORRECT
                            case 1: w = 60; h = 42; break;
                            case 2: w = 60; h = 42; break;
                            case 3: w = 60; h = 42; break;
                        }
                        break;
                    case 7:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 60; h = 34; break; // CORRECT
                            case 1: w = 60; h = 34; break;
                            case 2: w = 60; h = 34; break;
                            case 3: w = 60; h = 34; break;
                        }
                        break;
                    case 8:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 56; h = 39; break; // CORRECT
                            case 1: w = 56; h = 39; break;
                            case 2: w = 56; h = 39; break;
                            case 3: w = 56; h = 39; break;
                        }
                        break;
                    case 9:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 56; h = 38; break; // CORRECT
                            case 1: w = 56; h = 38; break;
                            case 2: w = 56; h = 38; break;
                            case 3: w = 56; h = 38; break;
                        }
                        break;
                    case 10:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 48; h = 31; break; // CORRECT
                            case 1: w = 48; h = 31; break;
                            case 2: w = 48; h = 31; break;
                            case 3: w = 48; h = 31; break;
                        }
                        break;
                }
            }
            else if (lastSelectedFile == "COLONIST") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0: w = 32; h = 56; break;
                    case 1: w = 32; h = 56; break;
                    case 2: w = 28; h = 57; break;
                    case 3: w = 40; h = 56; break;
                    case 4: w = 28; h = 57; break;
                    case 5: w = 32; h = 60; break;
                    case 6: w = 32; h = 60; break;
                    case 7: w = 24; h = 60; break;
                    case 8: w = 24; h = 60; break;
                    case 9: w = 40; h = 60; break;
                    case 10: w = 24; h = 58; break;
                    case 11: w = 40; h = 60; break;
                    case 12: w = 32; h = 59; break;
                    case 13: w = 32; h = 59; break;
                    case 14: w = 24; h = 58; break;
                    case 15:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 40; h = 60; break; // CORRECT
                            case 1: w = 40; h = 60; break;
                            case 2: w = 40; h = 60; break;
                        }
                        break;
                    case 16:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 36; h = 59; break; // CORRECT
                            case 1: w = 36; h = 59; break;
                            case 2: w = 36; h = 59; break;
                            case 3: w = 36; h = 59; break;
                        }
                        break;
                    case 17:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 40; h = 59; break; // CORRECT
                            case 1: w = 40; h = 59; break;
                            case 2: w = 40; h = 59; break;
                            case 3: w = 40; h = 59; break;
                        }
                        break;
                    case 18: w = 24; h = 42; break;
                    case 19: w = 36; h = 42; break;
                    case 20: w = 32; h = 41; break;
                    case 21: w = 32; h = 41; break;
                    case 22: w = 24; h = 42; break;
                }
            }
            else if (lastSelectedFile == "DOG") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 88; h = 101; break; // CORRECT
                            case 1: w = 88; h = 101; break;
                            case 2: w = 88; h = 101; break;
                            case 3: w = 88; h = 101; break;
                        }
                        break;
                    case 1:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 88; h = 52; break; // CORRECT
                            case 1: w = 88; h = 52; break;
                            case 2: w = 88; h = 52; break;
                            case 3: w = 88; h = 52; break;
                        }
                        break;
                    case 2:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 84; h = 98; break; // CORRECT
                            case 1: w = 84; h = 98; break;
                            case 2: w = 84; h = 98; break;
                            case 3: w = 84; h = 98; break;
                            case 4: w = 84; h = 98; break;
                            case 5: w = 84; h = 98; break;
                        }
                        break;
                    case 3:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 64; h = 92; break; // CORRECT
                            case 1: w = 64; h = 92; break;
                            case 2: w = 64; h = 92; break;
                            case 3: w = 64; h = 92; break;
                            case 4: w = 64; h = 92; break;
                            case 5: w = 64; h = 92; break;
                        }
                        break;
                    case 4:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 56; h = 78; break; // CORRECT
                            case 1: w = 56; h = 78; break;
                            case 2: w = 56; h = 78; break;
                            case 3: w = 56; h = 78; break;
                            case 4: w = 56; h = 78; break;
                            case 5: w = 56; h = 78; break;
                            case 6: w = 56; h = 78; break;
                            case 7: w = 56; h = 78; break;
                        }
                        break;
                    case 5:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 80; h = 79; break; // CORRECT
                            case 1: w = 80; h = 79; break;
                            case 2: w = 80; h = 79; break;
                            case 3: w = 80; h = 79; break;
                            case 4: w = 80; h = 79; break;
                            case 5: w = 80; h = 79; break;
                            case 6: w = 80; h = 79; break;
                            case 7: w = 80; h = 79; break;
                        }
                        break;
                    case 6:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 100; h = 82; break; // CORRECT
                            case 1: w = 100; h = 82; break;
                            case 2: w = 100; h = 82; break;
                            case 3: w = 100; h = 82; break;
                            case 4: w = 100; h = 82; break;
                            case 5: w = 100; h = 82; break;
                            case 6: w = 100; h = 82; break;
                            case 7: w = 100; h = 82; break;
                        }
                        break;
                    case 7:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 104; h = 86; break; // CORRECT
                            case 1: w = 104; h = 86; break;
                            case 2: w = 104; h = 86; break;
                            case 3: w = 104; h = 86; break;
                            case 4: w = 104; h = 86; break;
                            case 5: w = 104; h = 86; break;
                            case 6: w = 104; h = 86; break;
                            case 7: w = 104; h = 86; break;
                        }
                        break;
                    case 8:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 60; h = 86; break; // CORRECT
                            case 1: w = 60; h = 86; break;
                            case 2: w = 60; h = 86; break;
                            case 3: w = 60; h = 86; break;
                            case 4: w = 60; h = 86; break;
                            case 5: w = 60; h = 86; break;
                            case 6: w = 60; h = 86; break;
                            case 7: w = 60; h = 86; break;
                        }
                        break;
                    case 9:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 60; h = 92; break; // CORRECT
                            case 1: w = 60; h = 92; break;
                            case 2: w = 60; h = 92; break;
                            case 3: w = 60; h = 92; break;
                            case 4: w = 60; h = 92; break;
                            case 5: w = 60; h = 92; break;
                            case 6: w = 60; h = 92; break;
                            case 7: w = 60; h = 92; break;
                        }
                        break;
                    case 10:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 60; h = 92; break; // CORRECT
                            case 1: w = 60; h = 92; break;
                            case 2: w = 60; h = 92; break;
                            case 3: w = 60; h = 92; break;
                            case 4: w = 60; h = 92; break;
                            case 5: w = 60; h = 92; break;
                            case 6: w = 60; h = 92; break;
                            case 7: w = 60; h = 92; break;
                        }
                        break;
                }
            }
            else if (lastSelectedFile == "DOGCEIL") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 120; h = 29; break; // CORRECT
                            case 1: w = 120; h = 29; break;
                            case 2: w = 120; h = 29; break;
                            case 3: w = 120; h = 29; break;
                            case 4: w = 120; h = 29; break;
                            case 5: w = 120; h = 29; break;
                            case 6: w = 120; h = 29; break;
                            case 7: w = 120; h = 29; break;
                        }
                        break;
                    case 1:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 112; h = 28; break; // CORRECT
                            case 1: w = 112; h = 28; break;
                            case 2: w = 112; h = 28; break;
                            case 3: w = 112; h = 28; break;
                            case 4: w = 112; h = 28; break;
                            case 5: w = 112; h = 28; break;
                            case 6: w = 112; h = 28; break;
                            case 7: w = 112; h = 28; break;
                        }
                        break;
                    case 2:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 80; h = 113; break; // CORRECT
                            case 1: w = 80; h = 113; break;
                            case 2: w = 80; h = 113; break;
                        }
                        break;
                    case 3:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 120; h = 44; break; // CORRECT
                            case 1: w = 120; h = 44; break;
                            case 2: w = 120; h = 44; break;
                            case 3: w = 120; h = 44; break;
                            case 4: w = 120; h = 44; break;
                            case 5: w = 120; h = 44; break;
                        }
                        break;
                    case 4:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 92; h = 77; break; // CORRECT
                            case 1: w = 92; h = 77; break;
                            case 2: w = 92; h = 77; break;
                            case 3: w = 92; h = 77; break;
                            case 4: w = 92; h = 77; break;
                            case 5: w = 92; h = 77; break;
                        }
                        break;
                }
            }
            else if (lastSelectedFile == "EGGS") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 48; h = 56; break; // CORRECT
                            case 1: w = 48; h = 56; break;
                            case 2: w = 48; h = 56; break;
                        }
                        break;
                    case 1:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 48; h = 56; break; // CORRECT
                            case 1: w = 48; h = 56; break;
                            case 2: w = 48; h = 56; break;
                        }
                        break;
                    case 2:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 48; h = 63; break; // CORRECT
                            case 1: w = 48; h = 63; break;
                            case 2: w = 48; h = 63; break;
                            case 3: w = 48; h = 63; break;
                            case 4: w = 48; h = 63; break;
                        }
                        break;
                    case 3:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 52; h = 30; break; // CORRECT
                            case 1: w = 52; h = 30; break;
                            case 2: w = 52; h = 30; break;
                            case 3: w = 52; h = 30; break;
                            case 4: w = 52; h = 30; break;
                            case 5: w = 52; h = 30; break;
                        }
                        break;
                    case 4:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 48; h = 62; break; // CORRECT
                            case 1: w = 48; h = 62; break;
                            case 2: w = 48; h = 62; break;
                            case 3: w = 48; h = 62; break;
                            case 4: w = 48; h = 62; break;
                            case 5: w = 48; h = 62; break;
                            case 6: w = 48; h = 62; break;
                            case 7: w = 48; h = 62; break;
                        }
                        break;
                    case 5:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 48; h = 57; break; // CORRECT
                            case 1: w = 48; h = 57; break;
                            case 2: w = 48; h = 57; break;
                            case 3: w = 48; h = 57; break;
                            case 4: w = 48; h = 57; break;
                            case 5: w = 48; h = 57; break;
                            case 6: w = 48; h = 57; break;
                            case 7: w = 48; h = 57; break;
                        }
                        break;
                }
            }
            else if (lastSelectedFile == "FINGERS") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 120; h = 43; break; // CORRECT
                            case 1: w = 120; h = 43; break;
                            case 2: w = 120; h = 43; break;
                            case 3: w = 120; h = 43; break;
                        }
                        break;
                    case 1:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 120; h = 45; break; // CORRECT
                            case 1: w = 120; h = 45; break;
                            case 2: w = 120; h = 45; break;
                            case 3: w = 120; h = 45; break;
                        }
                        break;
                }
            }
            else if (lastSelectedFile == "GUARD") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 56; h = 120; break; // CORRECT
                            case 1: w = 56; h = 120; break;
                            case 2: w = 56; h = 120; break;
                            case 3: w = 56; h = 120; break;
                        }
                        break;
                    case 1:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 44; h = 113; break; // CORRECT
                            case 1: w = 44; h = 113; break;
                            case 2: w = 44; h = 113; break;
                            case 3: w = 44; h = 113; break;
                            case 4: w = 44; h = 113; break;
                            case 5: w = 44; h = 113; break;
                        }
                        break;
                    case 2:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 32; h = 115; break; // CORRECT
                            case 1: w = 32; h = 115; break;
                            case 2: w = 32; h = 115; break;
                        }
                        break;
                    case 3:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 56; h = 119; break; // CORRECT
                            case 1: w = 56; h = 119; break;
                            case 2: w = 56; h = 119; break;
                            case 3: w = 56; h = 119; break;
                            case 4: w = 56; h = 119; break;
                            case 5: w = 56; h = 119; break;
                            case 6: w = 56; h = 119; break;
                            case 7: w = 56; h = 119; break;
                        }
                        break;
                    case 4:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 56; h = 115; break; // CORRECT
                            case 1: w = 56; h = 115; break;
                            case 2: w = 56; h = 115; break;
                            case 3: w = 56; h = 115; break;
                            case 4: w = 56; h = 115; break;
                            case 5: w = 56; h = 115; break;
                            case 6: w = 56; h = 115; break;
                            case 7: w = 56; h = 115; break;
                        }
                        break;
                    case 5:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 56; h = 116; break; // CORRECT
                            case 1: w = 56; h = 116; break;
                            case 2: w = 56; h = 116; break;
                            case 3: w = 56; h = 116; break;
                            case 4: w = 56; h = 116; break;
                            case 5: w = 56; h = 116; break;
                            case 6: w = 56; h = 116; break;
                            case 7: w = 56; h = 116; break;
                        }
                        break;
                    case 6:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 64; h = 118; break; // CORRECT
                            case 1: w = 64; h = 118; break;
                            case 2: w = 64; h = 118; break;
                            case 3: w = 64; h = 118; break;
                            case 4: w = 64; h = 118; break;
                            case 5: w = 64; h = 118; break;
                            case 6: w = 64; h = 118; break;
                            case 7: w = 64; h = 118; break;
                        }
                        break;
                    case 7:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 56; h = 114; break; // CORRECT
                            case 1: w = 56; h = 114; break;
                            case 2: w = 56; h = 114; break;
                            case 3: w = 56; h = 114; break;
                            case 4: w = 56; h = 114; break;
                            case 5: w = 56; h = 114; break;
                            case 6: w = 56; h = 114; break;
                            case 7: w = 56; h = 114; break;
                        }
                        break;
                }
            }
            else if (lastSelectedFile == "HANDLER") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 64; h = 101; break; // CORRECT
                            case 1: w = 64; h = 101; break;
                            case 2: w = 64; h = 101; break;
                            case 3: w = 64; h = 101; break;
                            case 4: w = 64; h = 101; break;
                            case 5: w = 64; h = 101; break;
                            case 6: w = 64; h = 101; break;
                            case 7: w = 64; h = 101; break;
                        }
                        break;
                    case 1:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 52; h = 101; break; // CORRECT
                            case 1: w = 52; h = 101; break;
                            case 2: w = 52; h = 101; break;
                            case 3: w = 52; h = 101; break;
                            case 4: w = 52; h = 101; break;
                            case 5: w = 52; h = 101; break;
                            case 6: w = 52; h = 101; break;
                            case 7: w = 52; h = 101; break;
                        }
                        break;
                    case 2:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 48; h = 102; break; // CORRECT
                            case 1: w = 48; h = 102; break;
                            case 2: w = 48; h = 102; break;
                            case 3: w = 48; h = 102; break;
                            case 4: w = 48; h = 102; break;
                            case 5: w = 48; h = 102; break;
                            case 6: w = 48; h = 102; break;
                            case 7: w = 48; h = 102; break;
                        }
                        break;
                    case 3:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 56; h = 103; break; // CORRECT
                            case 1: w = 56; h = 103; break;
                            case 2: w = 56; h = 103; break;
                            case 3: w = 56; h = 103; break;
                            case 4: w = 56; h = 103; break;
                            case 5: w = 56; h = 103; break;
                            case 6: w = 56; h = 103; break;
                            case 7: w = 56; h = 103; break;
                        }
                        break;
                    case 4:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 68; h = 102; break; // CORRECT
                            case 1: w = 68; h = 102; break;
                            case 2: w = 68; h = 102; break;
                            case 3: w = 68; h = 102; break;
                            case 4: w = 68; h = 102; break;
                            case 5: w = 68; h = 102; break;
                            case 6: w = 68; h = 102; break;
                            case 7: w = 68; h = 102; break;
                        }
                        break;
                    case 5:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 56; h = 104; break; // CORRECT
                            case 1: w = 56; h = 104; break;
                            case 2: w = 56; h = 104; break;
                            case 3: w = 56; h = 104; break;
                        }
                        break;
                    case 6:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 68; h = 100; break; // CORRECT
                            case 1: w = 68; h = 100; break;
                            case 2: w = 68; h = 100; break;
                            case 3: w = 68; h = 100; break;
                            case 4: w = 68; h = 100; break;
                            case 5: w = 68; h = 100; break;
                        }
                        break;
                    case 7:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 48; h = 100; break; // CORRECT
                            case 1: w = 48; h = 100; break;
                            case 2: w = 48; h = 100; break;
                            case 3: w = 48; h = 100; break;
                            case 4: w = 48; h = 100; break;
                            case 5: w = 48; h = 100; break;
                            case 6: w = 48; h = 100; break;
                            case 7: w = 48; h = 100; break;
                        }
                        break;
                    case 8:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 44; h = 100; break; // CORRECT
                            case 1: w = 44; h = 100; break;
                            case 2: w = 44; h = 100; break;
                        }
                        break;
                }
            }
            else if (lastSelectedFile == "HUGGER") // INCOMPLETE
            {
                switch (SelectedIndex)
                {
                    case 0:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 88; h = 19; break; // CORRECT
                            case 1: w = 88; h = 19; break;
                            case 2: w = 88; h = 19; break;
                            case 3: w = 88; h = 19; break;
                        }
                        break;
                    case 1:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 92; h = 30; break; // CORRECT
                            case 1: w = 92; h = 30; break;
                            case 2: w = 92; h = 30; break;
                            case 3: w = 92; h = 30; break;
                        }
                        break;
                    case 2:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 124; h = 26; break; // CORRECT
                            case 1: w = 124; h = 26; break;
                            case 2: w = 124; h = 26; break;
                        }
                        break;
                    case 3:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 92; h = 30; break; // CORRECT
                            case 1: w = 92; h = 30; break;
                            case 2: w = 92; h = 30; break;
                            case 3: w = 92; h = 30; break;
                        }
                        break;
                    case 4:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 48; h = 37; break; // CORRECT
                            case 1: w = 48; h = 37; break;
                            case 2: w = 48; h = 37; break;
                            case 3: w = 48; h = 37; break;
                        }
                        break;
                    case 5:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 64; h = 32; break; // CORRECT
                            case 1: w = 64; h = 32; break;
                            case 2: w = 64; h = 32; break;
                            case 3: w = 64; h = 32; break;
                            case 4: w = 64; h = 32; break;
                            case 5: w = 64; h = 32; break;
                            case 6: w = 64; h = 32; break;
                        }
                        break;
                    case 6:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 76; h = 40; break; // CORRECT
                            case 1: w = 76; h = 40; break;
                            case 2: w = 76; h = 40; break;
                            case 3: w = 76; h = 40; break;
                            case 4: w = 76; h = 40; break;
                            case 5: w = 76; h = 40; break;
                        }
                        break;
                    case 7:
                        switch (FrameIndex) // INCOMPLETE
                        {
                            case 0: w = 48; h = 62; break; // CORRECT
                            case 1: w = 48; h = 62; break;
                            case 2: w = 48; h = 62; break;
                            case 3: w = 48; h = 62; break;
                            case 4: w = 48; h = 62; break;
                            case 5: w = 48; h = 62; break;
                            case 6: w = 48; h = 62; break;
                            case 7: w = 48; h = 62; break;
                        }
                        break;
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