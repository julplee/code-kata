using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Kata
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void IntegrationTest()
        {
            var lines = File.ReadLines("dictionnaire.txt", Encoding.Default).ToList();

            var linesWithoutAccent = new HashSet<string>(lines.Select(p => Encoding.ASCII.GetString(Encoding.GetEncoding("iso-8859-8").GetBytes(p)))).ToList();
            var dico = PrecomputeDico(linesWithoutAccent);

            string input = "Certaines personnes compressent le texte des messages en ne conservant que les voyelles qui débutent un mot en supprimant les accentuations et en remplaçant les lettres doublées par des lettres simples";
            input = Encoding.ASCII.GetString(Encoding.GetEncoding("iso-8859-8").GetBytes(input));

            var cut = CutSentence(input);
            var cutCompressed = cut.Select(p => Compress(p));


            var output = DecompressSentence(dico, string.Join(" ", cutCompressed));
        }

        [Test]
        public void SimpleCase()
        {
            string testWord = "ni";

            string compressed = Compress(testWord);

            Assert.AreEqual("n", compressed);
        }

        [Test]
        public void OneVowel()
        {
            string testWord = "thon";

            string compressed = Compress(testWord);

            Assert.AreEqual("thn", compressed);
        }


        [Test]
        public void SimpleCaseAllVowels()
        {
            string testWord = "taeiouy";

            string compressed = Compress(testWord);

            Assert.AreEqual("t", compressed);
        }

        [Test]
        public void DoubleLettersAreRemoved()
        {
            string testWord = "tt";

            string compressed = Compress(testWord);

            Assert.AreEqual("t", compressed);
        }

        [Test]
        public void DoubleLettersAreNotRemovedIfTheyAreNotContiguous()
        {
            string testWord = "tête";

            string compressed = Compress(testWord);

            Assert.AreEqual("tt", compressed);
        }

        [Test]
        public void SimpleCaseAllAccents()
        {
            string testWord = "tàâêèéû";

            string compressed = Compress(testWord);

            Assert.AreEqual("t", compressed);
        }


        [Test]
        public void FirstLetterIsAVowel()
        {
            string testWord = "om";

            string compressed = Compress(testWord);

            Assert.AreEqual("om", compressed);
        }

        [Test]
        public void CanDeCompressSimpleWord()
        {
            var dico = new List<string> { "wagon", "toto" };
            string testCompressed = "wgn";

            Assert.AreEqual(new[] { "wagon" }, DecompressWord(PrecomputeDico(dico), testCompressed));
        }

        [Test]
        public void TestCutSentence()
        {
            string testSentence = "le wagon de tête";

            Assert.AreEqual(new[] { "le","wagon","de","tête" }, CutSentence(testSentence));
        }

        [Test]
        public void CanDeCompressSentence()
        {
            var dico = new List<string> { "le", "wagon", "de", "tête" };
            string testSentence = "l wgn d tt";

            Assert.AreEqual("le wagon de tête", DecompressSentence(PrecomputeDico(dico), testSentence));
        }

        [Test]
        public void CanDeCompressSentenceWithOptions()
        {
            var dico = new List<string> { "le", "wagon", "de", "tête", "toto" };
            string testSentence = "l wgn d tt";

            Assert.AreEqual("le wagon de (tête toto)", DecompressSentence(PrecomputeDico(dico), testSentence));
        }

        [Test]
        public void CanPrecomputeDictionary()
        {
            var dico = new List<string> {"toto", "titi", "tutu", "arbre"};
            var result = PrecomputeDico(dico);
            CollectionAssert.AreEquivalent(new string[] {"toto", "titi", "tutu"}, result["tt"]);
        }

        private string[] CutSentence(string testSentence)
        {
            return testSentence.Split(' ');
        }

        private string DecompressSentence(Dictionary<string, List<string>> dico, string sentenceToDeCompress)
        {
            var cutSentence = CutSentence(sentenceToDeCompress);
            return string.Join(" ", cutSentence.Select(p => FormatResult(dico[p])));
        }

        private string FormatResult(List<string> list)
        {
            if (list.Count == 1)
                return list[0];

            return "(" + string.Join(" ", list) + ")";
        }

        private List<string> DecompressWord(Dictionary<string, List<string>> dico, string wordToCompress)
        {
            var compressed = Compress(wordToCompress);

            return dico[compressed];
        }

        private Dictionary<string, List<string>> PrecomputeDico(List<string> dico)
        {
            var grouped = dico.GroupBy(x => Compress(x)).ToDictionary(x => x.Key, x => x.ToList(), StringComparer.OrdinalIgnoreCase);
            return grouped;
        }


        private string Compress(string testWord)
        {
            if (vowels.Contains(testWord.First()))
            {
                char firstVowel = testWord.First();

                return firstVowel + RemoveVowels(RemoveDouble(testWord));
            }

            return RemoveVowels(RemoveDouble(testWord));
        }

        private string RemoveDouble(string source)
        {
            StringBuilder sb = new StringBuilder();

            char previous = '\0';
            foreach (char c in source)
            {
                if (c != previous)
                {
                    sb.Append(c);
                }

                previous = c;
            }

            return sb.ToString();
        }

        readonly char[] vowels = new[] { 'a', 'e', 'i', 'o', 'u', 'y', 'à', 'â', 'ä', 'ê', 'é', 'è', 'ë', 'û', 'ù', 'ü', 'î', 'ï', 'ô', 'ö' };

        public string RemoveVowels(IEnumerable<char> characters)
        {
            return new string(characters.Where(c => !vowels.Contains(c)).ToArray());
        }
    }
}
