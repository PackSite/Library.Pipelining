namespace PackSite.Library.Pipelining
{
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

            if (name.Any(x => x is ':' or '_'))
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
            return obj is PipelineName itemName && Equals(itemName);
        }

        /// <inheritdoc/>
        public bool Equals(PipelineName? other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other is not null && Value.Equals(other.Value, StringComparison.Ordinal);
        }

        /// <summary>
        /// Determines whether two specified value objects have the same value.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(PipelineName left, PipelineName right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two specified value objects have different values.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(PipelineName left, PipelineName right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        public int CompareTo(PipelineName? other)
        {
            if (other is null)
            {
                return -1;
            }

            return string.CompareOrdinal(Value, other.Value);
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
            return Value.GetHashCode(StringComparison.Ordinal);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Value;
        }
    }
}
