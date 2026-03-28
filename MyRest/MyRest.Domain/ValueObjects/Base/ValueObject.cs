#nullable disable
using System;
using MyRest.Domain.ValueObjects.Exceptions;

namespace MyRest.Domain.ValueObjects.Base
{
    public class ValueObject<T> : IEquatable<ValueObject<T>>
    {
        public T Value { get; set; }

        protected ValueObject()
        {
        }

        protected ValueObject(IValidator<T> validator, T value)
        {
            if (validator is null)
                throw new ValidatorNullException(nameof(validator));

            validator.Validate(value);
            Value = value;
        }

        public override string ToString() => Value?.ToString() ?? GetType().ToString();

        public override bool Equals(object obj) => Equals(obj as ValueObject<T>);

        public bool Equals(ValueObject<T> other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (Value is null && other.Value is null) return true;
            if (Value is null) return false;
            return Value.Equals(other.Value);
        }

        public override int GetHashCode() => Value?.GetHashCode() ?? 0;

        public static bool operator ==(ValueObject<T> left, ValueObject<T> right) => Equals(left, right);
        public static bool operator !=(ValueObject<T> left, ValueObject<T> right) => !(left == right);
    }
}