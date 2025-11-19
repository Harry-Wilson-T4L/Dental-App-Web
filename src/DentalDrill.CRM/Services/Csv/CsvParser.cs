using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Services.Csv
{
    public static class CsvParser
    {
        public static IEnumerable<T> Parse<T>(TextReader reader, Char delimiter, Char qualifier)
        {
            foreach (var entry in CsvParser.Parse(reader, delimiter, qualifier))
            {
                yield return entry.ParseAs<T>();
            }
        }

        public static IEnumerable<CsvEntry> Parse(TextReader reader, Char delimiter, Char qualifier)
        {
            var lines = CsvParser.ParseLines(reader, delimiter, qualifier);
            using (var enumerator = lines.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    throw new InvalidOperationException("Unable to parse csv header");
                }

                var headerLine = enumerator.Current;
                var header = new CsvHeader(headerLine);

                while (enumerator.MoveNext())
                {
                    var line = enumerator.Current;
                    yield return new CsvEntry(header, line);
                }
            }
        }

        private static IEnumerable<IList<String>> ParseLines(TextReader reader, Char delimiter, Char qualifier)
        {
            var inQuote = false;
            var record = new List<String>();
            var sb = new StringBuilder();

            while (reader.Peek() != -1)
            {
                var readChar = (Char)reader.Read();

                if (readChar == '\n' || (readChar == '\r' && (Char)reader.Peek() == '\n'))
                {
                    // If it's a \r\n combo consume the \n part and throw it away.
                    if (readChar == '\r')
                    {
                        reader.Read();
                    }

                    if (inQuote)
                    {
                        if (readChar == '\r')
                        {
                            sb.Append('\r');
                        }

                        sb.Append('\n');
                    }
                    else
                    {
                        if (record.Count > 0 || sb.Length > 0)
                        {
                            record.Add(sb.ToString());
                            sb.Clear();
                        }

                        if (record.Count > 0)
                        {
                            yield return record;
                        }

                        record = new List<String>(record.Count);
                    }
                }
                else if (sb.Length == 0 && !inQuote)
                {
                    if (readChar == qualifier)
                    {
                        inQuote = true;
                    }
                    else if (readChar == delimiter)
                    {
                        record.Add(sb.ToString());
                        sb.Clear();
                    }
                    else if (Char.IsWhiteSpace(readChar))
                    {
                        // Ignore leading whitespace
                    }
                    else
                    {
                        sb.Append(readChar);
                    }
                }
                else if (readChar == delimiter)
                {
                    if (inQuote)
                    {
                        sb.Append(delimiter);
                    }
                    else
                    {
                        record.Add(sb.ToString());
                        sb.Clear();
                    }
                }
                else if (readChar == qualifier)
                {
                    if (inQuote)
                    {
                        if ((Char)reader.Peek() == qualifier)
                        {
                            reader.Read();
                            sb.Append(qualifier);
                        }
                        else
                        {
                            inQuote = false;
                        }
                    }
                    else
                    {
                        sb.Append(readChar);
                    }
                }
                else
                {
                    sb.Append(readChar);
                }
            }

            if (record.Count > 0 || sb.Length > 0)
            {
                record.Add(sb.ToString());
            }

            if (record.Count > 0)
            {
                yield return record;
            }
        }
    }
}
