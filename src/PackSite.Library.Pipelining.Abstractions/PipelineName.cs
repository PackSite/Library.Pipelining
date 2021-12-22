namespace PackSite.Library.Pipelining
{
    using System;
    using System.Linq;

    /// <summary>
    /// Pipeline name.
    /// </summary>
    public sealed class PipelineName : IEquatable<PipelineName>, IComparable<PipelineName>, IComparable
    {
        /// <summary>
        /// Item name.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="PipelineName"/>.
        /// </summary>
        /// <param name="name"></param>
        public PipelineName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            if (name.Where(x => x is ':' or '_').Any())
            {
                throw new ArgumentException($"'{nameof(name)}' cannot contain ':' and '_'.", nameof(name));
            }

            Value = name;
        }

        /// <summary>
        /// Converts <see cref="string"/> to <see cref="PipelineName"/>.
        /// </summary>
        /// <param name="name"></param>
        public static implicit operator PipelineName(string name)
        {
            return new PipelineName(name);
        }

        /// <summary>
        /// Converts <see cref="PipelineName"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="itemName"></param>
        public static implicit operator string(PipelineName itemName)
        {
            return itemName.Value;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is PipelineName itemName && Value.Equals(itemName.Value);
        }

        /// <inheritdoc/>
        public bool Equals(PipelineName? other)
        {
            return other is not null && Value.Equals(other.Value);
        }

        /// <summary>
        /// Determines whether two specified value objects have the same value.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(PipelineName left, PipelineName right)
        {
            return left.Value.Equals(right.Value);
        }

        /// <summary>
        /// Determines whether two specified value objects have different values.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(PipelineName left, PipelineName right)
        {
            return !left.Value.Equals(right.Value);
        }

        /// <inheritdoc/>
        public int CompareTo(PipelineName? other)
        {
            if (other is null)
            {
                return -1;
            }

            return Value.CompareTo(other.Value);
        }

        /// <inheritdoc/>
        public int CompareTo(object? obj)
        {
            if (obj is not PipelineName other)
            {
                return -1;
            }

            return CompareTo(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
