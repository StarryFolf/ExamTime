using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Question
    {
        internal Question(string text)
        {
            Text = text;
        }
        internal string Text { get; }
    }

    internal sealed class QuestionList : IEnumerable<Question>
    {
        private List<Question> _list;

        internal QuestionList(List<Question> list)
        {
            _list = list;
        }

        internal void Add(Question question)
        {
            _list.Add(question);
        }

        public IEnumerator<Question> GetEnumerator()
        {
            return new QuestionEnumerator(_list);
        }

        private IEnumerator GetEnumerator1()
        {
            return this.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator1();
        }
    }

    internal sealed class QuestionEnumerator : IEnumerator<Question>
    {
        public List<Question> _list;
        private int index = -1;

        public QuestionEnumerator(List<Question> list)
        {
            _list = list;
        }

        public Question Current
        {
            get
            {
                return _list[index];
            }
        }

        public void Reset()
        {
            index = -1;
        }

        public bool MoveNext()
        {
            return ++index < _list.Count;
        }

        void IDisposable.Dispose() { }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }
    }

    internal sealed class QuestionMetadata : IEquatable<QuestionMetadata>
    {
        internal Categories Category { get; }
        internal Difficulties Difficulty { get; }
        internal QuestionMetadata(Categories category, Difficulties difficulty)
        {
            Category = category;
            Difficulty = difficulty;
        }
        public bool Equals(QuestionMetadata other)
        {
            return other != null && Category == other.Category && Difficulty == other.Difficulty;
        }
        public override bool Equals(object other)
        {
            return Equals(other as QuestionMetadata);
        }
        public override int GetHashCode()
        {
            return (int)Category * 104729 + (int)Difficulty * 104723 + 104717;
        }
    }

    internal sealed class QuestionBank
    {
        private readonly Dictionary<QuestionMetadata, QuestionList> _bank = new Dictionary<QuestionMetadata, QuestionList>();
        internal QuestionList this[QuestionMetadata metadata]
        {
            get
            {
                Create(metadata);
                return _bank[metadata];
            }
        }
        internal void Add(Question question, QuestionMetadata metadata)
        {
            Create(metadata);
            _bank[metadata].Add(question);
        }
        private void Create(QuestionMetadata metadata)
        {
            if (!_bank.ContainsKey(metadata))
                _bank.Add(metadata, new QuestionList(new List<Question>()));
        }
    }

    internal enum Difficulties
    {
        Easy, Medium, Hard
    }

    internal enum Categories
    {
        Algebra, Geometry, Physics, Chemistry
    }