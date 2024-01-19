using TMPro;
using UnityEngine;

namespace JamesFrowen.ScriptableVariables
{
    public class NonAllocGui
    {
        private const char EMPTY = ' ';
        private const char OUT_OF_BOUNDS = 'x';
        private static readonly char[] Numbers = new char[]
        {
            '0',
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
        };

        public readonly char[] Chars;
        private readonly int _start;
        private readonly int _digits; // numbers left of decimal
        private readonly int _decimals;
        private readonly float _max;
        private readonly float _rounder;

        public NonAllocGui(Wrapper settings) : this(settings.Format, settings.Start, settings.Digits, settings.Decimals) { }
        public NonAllocGui(string format, int start, int digits, int decimals)
        {
            Chars = format.ToCharArray();
            _start = start;
            _digits = digits;
            _decimals = decimals;
            _max = Mathf.Pow(10, digits);
            // will cause value to round to nearest int, not just floor
            // eg 10^(-1) = 0.1 => 0.1 /2 => 0.05
            _rounder = Mathf.Pow(10, -decimals) / 2;
        }

        public char[] GetChars(double value)
        {
            if (value < 0)
                throw new System.ArgumentOutOfRangeException(nameof(value), "Value must be positive");

            // add rounder so that value will round to nearest int, not just floor
            value += _rounder;

            if (value >= _max)
                SetOutOfBounds();
            else
                SetValue(value);

            return Chars;
        }

        private void SetOutOfBounds()
        {
            var offset = _start;
            for (var i = 0; i < _digits; i++)
            {
                Chars[offset + i] = OUT_OF_BOUNDS;
            }

            offset = _start + _digits + 1; // skip one for decimal place
            for (var i = 0; i < _decimals; i++)
            {
                Chars[offset + i] = OUT_OF_BOUNDS;
            }
        }

        private void SetValue(double value)
        {
            var offset = _start;
            var multiplier = _max;
            var total = 0;
            for (var i = 0; i < _digits; i++)
            {
                multiplier /= 10;
                var next = ((int)(value / multiplier)) % 10;
                total += next;

                // if not last digit, and total is 0, then write emtpy
                if (i < _digits - 1 && total == 0)
                    Chars[offset + i] = EMPTY;
                else
                    Chars[offset + i] = Numbers[next];
            }

            offset = _start + _digits + 1; // skip one for decimal place
            for (var i = 0; i < _decimals; i++)
            {
                multiplier /= 10;
                var next = ((int)(value / multiplier)) % 10;

                Chars[offset + i] = Numbers[next];
            }
        }

        [System.Serializable]
        public class Wrapper
        {
            public TextMeshProUGUI Text;

            [Tooltip("Format of string")]
            public string Format;

            [Tooltip("What position to start editing string")]
            public int Start;

            [Tooltip("How many digits to edit")]
            public int Digits;

            [Tooltip("How many Decimal places to edit. Will skip 1 char after Digits before Decimals")]
            public int Decimals;

            private NonAllocGui _helper;

            public void Init()
            {
                _helper = new NonAllocGui(this);
            }

            public void SetValue(float value)
            {
                if (_helper is null)
                    _helper = new NonAllocGui(this);

                Text.SetText(_helper.GetChars(value));
            }
        }
    }
}
