using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DotFuzzy
{
    /// <summary>
    /// Represents a rule.
    /// </summary>
    /// 
    public class FuzzyRule
    {
        private string text = String.Empty;
        private double value = 0;

        private string Validate(string text)
        {
            int count = 0;
            int position = text.IndexOf("(");
            string[] tokens = text.Replace("(", "").Replace(")", "").Split();

            while (position >= 0)
            {
                count++;
                position = text.IndexOf("(", position + 1);
            }

            position = text.IndexOf(")");
            while (position >= 0)
            {
                count--;
                position = text.IndexOf(")", position + 1);
            }

            if (count > 0)
                throw new Exception("missing right parenthesis: " + text);
            else if (count < 0)
                throw new Exception("missing left parenthesis: " + text);

            if (tokens[0] != "IF")
                throw new Exception("'IF' not found: " + text);

            if (tokens[tokens.Length - 4] != "THEN")
                throw new Exception("'THEN' not found: " + text);

            if (tokens[tokens.Length - 2] != "IS")
                throw new Exception("'IS' not found: " + text);

            for (int i = 2; i < (tokens.Length - 5); i = i + 2)
            {
                if ((tokens[i] != "IS") && (tokens[i] != "AND") && (tokens[i] != "OR"))
                    throw new Exception("Syntax error: " + tokens[i]);
            }

            return text;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public FuzzyRule()
        {
        }

        /// <param name="text">The text of the rule.</param>
        public FuzzyRule(string text)
        {
            this.Text = text;
        }

        /// <summary>
        /// The text of the rule.
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = Validate(value); }
        }

        /// <summary>
        /// The value of the rule after the evaluation process.
        /// </summary>
        public double Value
        {
            get { return value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Returns the conditions of the rule.    
        /// The part of the rule between IF and THEN.
        /// </summary>
        /// <returns>The conditions of the rule.</returns>
        public string Conditions()
        {
            return this.text.Substring(this.text.IndexOf("IF ") + 3, this.text.IndexOf(" THEN") - 3);
        }

    }
}
