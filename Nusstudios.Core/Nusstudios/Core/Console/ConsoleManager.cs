using System;
using System.Collections.Generic;

namespace Nusstudios.Core.Console
{
    public class ConsoleManager
    {
        private string allText = "";
        private int textOriginalConsoleLeft;
        private int textOriginalConsoleTop;
        private int consoleCharWidth;
        private int consoleCharHeight;
        private bool updateNextWrite = false;
        private bool updateThisWrite = false;
        internal Object _lock = new Object();

        public void UpdateNextWrite()
        {
            lock (_lock)
            {
                updateNextWrite = true;
            }
        }

        public void DisableUpdate()
        {
            lock (_lock)
            {
                updateNextWrite = false;
                updateThisWrite = false;
            }
        }

        public ConsoleManager(int originLeft, int originTop, int consoleWidth, int consoleHeight)
        {
            textOriginalConsoleLeft = originLeft;
            textOriginalConsoleTop = originTop;
            consoleCharWidth = consoleWidth;
            consoleCharHeight = consoleHeight;
        }

        private struct Coords
        {
            public int x;
            public int y;
        }

        private struct TextIndex
        {
            public int _id;
            public int _index;
            public int _length;
        }

        private List<TextIndex> indices = new List<TextIndex>();

        public void ClearLast(bool unindex, bool setBackCursor)
        {
            ClearID(LargestID(), unindex, setBackCursor);
        }

        public void ClearID(int ID, bool unindex, bool setBackCursor)
        {
            TextIndex index = GetIndex(ID);
            OverwriteOrAppend(index._index, new string(' ', index._length));

            if (unindex)
            {
                indices.Remove(index);
            }

            int rememberConsoleLeft = System.Console.CursorLeft;
            int rememberConsoleTop = System.Console.CursorTop;
            Coords seekedCoords = TextIndexToConsoleCoords(index._index);
            System.Console.SetCursorPosition(seekedCoords.x, seekedCoords.y);
            System.Console.Write(new string(' ', index._length));

            if (setBackCursor)
            {
                System.Console.SetCursorPosition(rememberConsoleLeft, rememberConsoleTop);
            }
            else
            {
                System.Console.SetCursorPosition(seekedCoords.x, seekedCoords.y);
            }
        }

        private TextIndex GetIndex(int id)
        {
            foreach (TextIndex index in indices)
            {
                if (index._id == id)
                {
                    return index;
                }
            }

            throw new Exception();
        }

        private int LargestID()
        {
            int largest = -1;

            if (indices.Count != 0)
            {
                foreach (TextIndex index in indices)
                {
                    if (index._id > largest)
                    {
                        largest = index._id;
                    }
                }
            }

            return largest;
        }

        private Coords TextIndexToTextCoords(int seekIndex)
        {
            int allTextEndLeft = textOriginalConsoleLeft;
            int allTextEndTop = textOriginalConsoleTop;

            for (int i = 0; i < seekIndex; i++)
            {
                char c = allText[i];

                if (allTextEndLeft == System.Console.BufferWidth - 1)
                {
                    allTextEndTop++;
                    allTextEndLeft = 0;
                }
                else
                {
                    allTextEndLeft++;
                }
            }

            return new Coords { x = allTextEndLeft, y = allTextEndTop };
        }

        private Coords TextEndCoords
        {
            get
            {
                return TextIndexToTextCoords(allText.Length - 1);
            }
        }

        // positive distance means coords2 is after coords1
        private int CharDistanceBetween(Coords coords1, Coords coords2)
        {
            int distanceFromOrigin1 = (coords1.y * consoleCharWidth) + coords1.x + 1;
            int distanceFromOrigin2 = (coords2.y * consoleCharWidth) + coords2.x + 1;
            return distanceFromOrigin2 - distanceFromOrigin1;
        }

        private Coords ConsoleCoordsAfterText()
        {
            Coords allTextEndConsoleCoords = TextIndexToConsoleCoords(allText.Length - 1);
            int _x;
            int _y;

            if ((allTextEndConsoleCoords.x + 1) == (consoleCharWidth - 1))
            {
                _y = allTextEndConsoleCoords.y + 1;
                _x = 0;
            }
            else
            {
                _y = allTextEndConsoleCoords.y;
                _x = allTextEndConsoleCoords.x + 1;
            }

            return new Coords { x = _x, y = _y };
        }

        private Coords TextIndexToConsoleCoords(int index)
        {
            Coords textCoords = TextIndexToTextCoords(index);

            if ((TextEndCoords.y + 1) > consoleCharHeight)
            {
                int textHiddenOverflowHeight = (TextEndCoords.y + 1) - consoleCharHeight;
                return new Coords { x = textCoords.x, y = textCoords.y - textHiddenOverflowHeight };
            }
            else
            {
                return textCoords;
            }
        }

        private void OverwriteOrAppend(int index, string insertText)
        {

            int insertLength = insertText.Length;
            int lengthFromIndex = allText.Length - index;

            if (lengthFromIndex >= insertLength)
            {
                allText = allText.Remove(index, insertText.Length).Insert(index, insertText);
            }
            else
            {
                // int toReplace = insertLength - lengthFromIndex;

                if (lengthFromIndex == 0)
                {
                    allText = allText + insertText;
                }
                else
                {
                    allText = allText.Remove(index, lengthFromIndex) + insertText;
                }
            }
        }

        // It is OK/fine/right for the returned text index to be out of range
        private int ConsoleCoordsToTextIndex()
        {
            return ConsoleCoordsToTextIndex(System.Console.CursorLeft, System.Console.CursorTop);
        }

        private int ConsoleCoordsToTextIndex(int consoleLeft, int consoleTop)
        {
            Coords _allTextEndCoords = TextEndCoords;
            int index;

            if ((_allTextEndCoords.y + 1) > consoleCharHeight)
            {
                // -1 from end!!!!
                int textHiddenOverflowHeight = (_allTextEndCoords.y + 1) - consoleCharHeight;
                int absoluteTop = textHiddenOverflowHeight + consoleTop;
                int absoluteLeft = consoleLeft;
                index = CharDistanceBetween(new Coords { x = textOriginalConsoleLeft, y = textOriginalConsoleTop }, new Coords { x = absoluteLeft, y = absoluteTop });
            }
            else
            {
                index = CharDistanceBetween(new Coords { x = textOriginalConsoleLeft, y = textOriginalConsoleTop }, new Coords { x = consoleLeft, y = consoleTop });
            }

            return index;
        }

        internal enum ReadType
        {
            ReadKey,
            ReadLine
        }

        internal int RegisteringRead(out string input, ReadType rt)
        {
            lock (_lock)
            {
                string buffer = "";
                int cursorLeft = System.Console.CursorLeft;
                int cursorTop = System.Console.CursorTop;
                int id;

                switch (rt)
                {
                    case ReadType.ReadKey:
                        ConsoleKeyInfo cki = System.Console.ReadKey(true);
                        buffer = Convert.ToString(cki.KeyChar);
                        input = buffer;

                        if (cki.Key == ConsoleKey.Enter)
                        {
                            if ((TextEndCoords.x + 1) != consoleCharWidth)
                            {
                                int zerosToAppendToEnd = consoleCharWidth - (TextEndCoords.x + 1);
                                cursorLeft = System.Console.CursorLeft;
                                cursorTop = System.Console.CursorTop;
                                // System.Console.Write(new string(' ', zerosToAppendToEnd));
                                System.Console.Write("\n");
                                id = Register(new string(' ', zerosToAppendToEnd), cursorLeft, cursorTop, true);
                            }
                            else
                            {
                                //System.Console.Write(new string(' ', consoleCharWidth));
                                System.Console.Write("\n");
                                id = Register(new string(' ', consoleCharWidth), cursorLeft, cursorTop, true);
                            }
                        }
                        else
                        {
                            id = Register(buffer, cursorLeft, cursorTop, true);
                        }

                        break;
                    default:
                        /* while (true)
                        {
                            var key = System.Console.ReadKey(true);

                            if (key.Key == ConsoleKey.Enter)
                            {
                                break;
                            }
                            else
                            {
                                buffer += key.KeyChar;
                                System.Console.Write(key.KeyChar);
                            }
                        }

                        id = Register(buffer, cursorLeft, cursorTop, true);
                        input = buffer;

                        if ((TextEndCoords.x + 1) != consoleCharWidth)
                        {
                            int zerosToAppendToEnd = consoleCharWidth - (TextEndCoords.x + 1);
                            cursorLeft = System.Console.CursorLeft;
                            cursorTop = System.Console.CursorTop;
                            System.Console.Write(new string(' ', zerosToAppendToEnd));
                            Register(new string(' ', zerosToAppendToEnd), cursorLeft, cursorTop, false);
                        }
                        else
                        {
                            System.Console.Write(new string(' ', consoleCharWidth));
                            Register(new string(' ', consoleCharWidth), cursorLeft, cursorTop, true);
                        } */

                        buffer = System.Console.ReadLine();
                        id = Register(buffer, cursorLeft, cursorTop, true);
                        input = buffer;
                        Coords consoleCoordsAfterText = ConsoleCoordsAfterText();
                        cursorTop = consoleCoordsAfterText.y;
                        cursorLeft = consoleCoordsAfterText.x;

                        if ((TextEndCoords.x + 1) != consoleCharWidth)
                        {
                            int zerosToAppendToEnd = consoleCharWidth - (TextEndCoords.x + 1);
                            // System.Console.Write(new string(' ', zerosToAppendToEnd));
                            Register(new string(' ', zerosToAppendToEnd), cursorLeft, cursorTop, false);
                        }
                        else
                        {
                            // System.Console.Write(new string(' ', consoleCharWidth));
                            Register(new string(' ', consoleCharWidth), cursorLeft, cursorTop, true);
                        }

                        break;
                }

                return id;
            }
        }

        private int Register(string text, int consoleInsertLeft, int consoleInsertTop, bool doIndex)
        {
            int textIndex = ConsoleCoordsToTextIndex(consoleInsertLeft, consoleInsertTop);
            int index = -1;

            if (textIndex < 0)
            {
                int overflow = textIndex + text.Length;

                if (overflow > 0)
                {
                    string overflowText = text.Substring(text.Length - overflow);
                    OverwriteOrAppend(0, overflowText);

                    if (doIndex)
                    {
                        index = IndexNew(0, overflowText.Length);
                    }
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (doIndex)
                {
                    index = IndexNew(textIndex, text.Length);
                }

                if ((allText.Length - 1) < textIndex)
                {
                    int spaceLength = (textIndex - 1) - (allText.Length - 1);
                    allText += new string(' ', spaceLength);
                    allText += text;
                }
                else
                {
                    OverwriteOrAppend(textIndex, text);
                }
            }

            return index;
        }

        internal int RegisteringWrite(string text, bool appendNewline)
        {
            lock (_lock)
            {
                if (updateThisWrite)
                {
                    ClearID(LargestID(), true, false);
                }
                else if (updateNextWrite)
                {
                    updateThisWrite = true;
                }

                int cursorLeft = System.Console.CursorLeft;
                int cursorTop = System.Console.CursorTop;
                int index = -1;

                if (!String.IsNullOrEmpty(text))
                {
                    System.Console.Write(text);
                    index = Register(text, cursorLeft, cursorTop, true);
                }

                if (appendNewline)
                {
                    int zerosToAppendToEnd = consoleCharWidth - (TextEndCoords.x + 1);
                    cursorLeft = System.Console.CursorLeft;
                    cursorTop = System.Console.CursorTop;
                    // System.Console.Write(new string(' ', zerosToAppendToEnd));
                    System.Console.Write("\n");

                    if (zerosToAppendToEnd != 0)
                    {
                        Register(new string(' ', zerosToAppendToEnd), cursorLeft, cursorTop, false);
                    }
                }

                return index;
            }
        }

        private int IndexNew(int index, int length)
        {
            int newID = LargestID() + 1;
            indices.Add(new TextIndex { _id = newID, _index = index, _length = length });
            return newID;
        }
    }
}
