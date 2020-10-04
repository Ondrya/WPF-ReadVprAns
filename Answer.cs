using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadVprAns
{
    /// <summary>
    /// Ответ пользователя
    /// </summary>
    public class Answer
    {
        public Answer(string id, string content, string[] realAnRealAnswers)
        {
            Id = id;
            Content = content;
            RealAnswers = realAnRealAnswers;
        }

        public string Id { get; set; }
        /// <summary>
        /// Строка с выбранными ответами из файла .ans
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Массив реальных ответов учеников, первый всегда вариант
        /// </summary>
        public string[] RealAnswers { get; set; }

        public override string ToString()
        {
            return Id;
        }
    }
}
