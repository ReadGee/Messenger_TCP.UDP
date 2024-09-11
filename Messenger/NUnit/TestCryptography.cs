namespace NUnit
{
    using Messenger.Cryptography;
    using NUnit.Framework;
    using NUnit;
    using System;
    using NUnit.Framework.Legacy;

    [TestFixture]
    public class Test_Cryptography
    {
        [TestCase("Default")]
        [TestCase("Login")]
        [TestCase("Password")]
        [TestCase("123")]
        [TestCase(" ")]
        [TestCase("")]
        public void Test_EncryptSHA512(string value)
        {
            ClassicAssert.IsTrue(Cryptography.EncryptSHA512(value).Length > value.Length);
            ClassicAssert.AreNotEqual(value, Cryptography.EncryptSHA512(value));
        }
        
        [Test]
        public void Test_RandomKeySha256Has()
        {
            ClassicAssert.IsNotNull(Cryptography.RandomKeySha256Hash());
        }

        [TestCase("Default", "85MJx8e9cSczqEyKuPWK6+AWmqs9K25+WsyE+iNzBX8=")]
        [TestCase("Error", "ZLe5SqA8z7EEgk0xy7naeMV6tyGVBo5lWLO3PDI6qwE=")]
        [TestCase("Sea", "VVZXSIJ5EcA63hYhhSb18Qhk51/jz32H2RV4Pm3rrLM=")]
        [TestCase("Username", "TEhVk4rJxRFFk+z+rdHJLUCjEVTiho7AQvLFpAduL5w=")]
        [TestCase("123", "rfmi11+OVu+wv6evf+XjJfGq4VDEP5ZxuF4whkvtgVE=")]
        [TestCase("%", "tjyqFuX5bORy+bng8hDivyLf0i9J1XPfWR/VnUsCU1c=")]
        [TestCase(" ", "vdEH2ILlr/eWw75M0dwE4Rp3pp7NquWBhjJPEPPS0i8=")]
        [TestCase("", "snDAvIik6U0eKutsFdMVQQAVUg0EzC5Fav4nt2x6PNA=")]
        public void Test_DefaultDecrypt(string answer, string text)
        {
            ClassicAssert.AreEqual(answer, Cryptography.DefaultDecrypt(text));            
        }

        [TestCase("Default")]
        [TestCase("Error")]
        [TestCase("Sea")]
        [TestCase("Username")]
        [TestCase("123")]
        [TestCase("%")]
        [TestCase(" ")]
        [TestCase("")]
        public void Test_DefaultEncrypt(string text)
        {
            ClassicAssert.IsNotNull(Cryptography.DefaultEncrypt(text));
            Console.WriteLine($"Text: {text}\nEncrypt: {Cryptography.DefaultEncrypt(text)}");
        }

        [TestCase("Default")]
        [TestCase("Error")]
        [TestCase("Sea")]
        [TestCase("Username")]
        [TestCase("123")]
        [TestCase("%")]
        [TestCase(" ")]
        [TestCase("")]
        public void Test_EncryptDecrypt(string text)
        {
            ClassicAssert.AreEqual(text, Cryptography.DefaultDecrypt(Cryptography.DefaultEncrypt(text)));
            Console.WriteLine($"Text: {text}\nEncrypt: {Cryptography.DefaultEncrypt(text)} \nDecrypt: {Cryptography.DefaultDecrypt(Cryptography.DefaultEncrypt(text))}");
        }
    }

    [TestFixture]
    public class TestRandom_Cryptography
    {
        const int MAX_COUNT_LOOP_EXECUTIONS = 50000;

        [TestCase('z', 'А')]
        [TestCase('А', 'z')]
        [TestCase('A', 'Z')]
        [TestCase('а', 'я')]
        [TestCase('А', 'Я')]
        [TestCase(' ', '@')]
        public void Test_EncryptDecrypt(char elem1, char elem2)
        {
            int count = 0;
            if(elem1 > elem2)
            {
                (elem1, elem2) = (elem2, elem1);
            }
            for(char letter = elem1; letter <= elem2; letter++)
            {
                ClassicAssert.AreEqual(letter.ToString(), Cryptography.DefaultDecrypt(Cryptography.DefaultEncrypt(letter.ToString())));
                Console.WriteLine("Letter: " + letter);
                if (MAX_COUNT_LOOP_EXECUTIONS == count++)
                {
                    ClassicAssert.Fail("Limit exceeded: MAX_COUNT_LOOP_EXECUTIONS");
                    break;
                }
            }
        }
    }
}
