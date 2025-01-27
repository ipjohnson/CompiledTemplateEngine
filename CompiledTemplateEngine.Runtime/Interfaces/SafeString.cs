﻿namespace CompiledTemplateEngine.Runtime.Interfaces;

/// <summary>
/// Escaped string ready to be used in rendering template
/// </summary>
public class SafeString(string? value) {
    private readonly string _value = value ?? "";

    public int Length => _value.Length;

    public override bool Equals(object? obj) {
        if (obj is SafeString safeString) {
            return _value.Equals(safeString._value);
        }

        return false;
    }

    public override int GetHashCode() {
        return _value.GetHashCode();
    }

    public override string ToString() {
        return _value;
    }

    public static implicit operator string(SafeString value) {
        return value._value;
    }

    public static SafeString operator +(SafeString safeString1, SafeString safeString2) {
        return new SafeString(safeString1._value + safeString2._value);
    }

    public static readonly SafeString Empty = new("");
}