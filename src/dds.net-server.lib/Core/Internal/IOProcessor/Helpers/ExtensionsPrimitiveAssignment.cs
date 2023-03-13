using DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Helpers
{
    internal static class ExtensionsPrimitiveAssignment
    {
        /// <summary>
        /// Assigns variable with given value.
        /// </summary>
        /// <param name="variable">The variable to be assigned.</param>
        /// <param name="value">Desired value.</param>
        /// <param name="errorMessage">Error message when value is not assigned.</param>
        /// <returns>True = value is changed, False = variable's last value is retained.</returns>
        /// <exception cref="Exception"></exception>
        internal static bool AssignPrimitiveString(this BasePrimitive variable, string value, out string errorMessage)
        {
            if (variable is StringVariable sv)
            {
                errorMessage = string.Empty;

                if (sv.Value != value)
                {
                    sv.Value = value;
                    return true;
                }

                return false;
            }
            else
            {
                errorMessage =
                    $"String value cannot be assigned to a variable " +
                    $"of type {variable.PrimitiveType}";

                return false;
            }
        }

        /// <summary>
        /// Assigns variable with given value.
        /// </summary>
        /// <param name="variable">The variable to be assigned.</param>
        /// <param name="value">Desired value.</param>
        /// <param name="errorMessage">Error message when value is not assigned.</param>
        /// <returns>True = value is changed, False = variable's last value is retained.</returns>
        /// <exception cref="Exception"></exception>
        internal static bool AssignPrimitiveBoolean(this BasePrimitive variable, bool value, out string errorMessage)
        {
            if (variable is BooleanVariable bv)
            {
                errorMessage = string.Empty;

                if (bv.Value != value)
                {
                    bv.Value = value;
                    return true;
                }

                return false;
            }
            else
            {
                errorMessage =
                    $"Boolean value cannot be assigned to a variable " +
                    $"of type {variable.PrimitiveType}";

                return false;
            }
        }

        /// <summary>
        /// Assigns variable with given value.
        /// </summary>
        /// <param name="variable">The variable to be assigned.</param>
        /// <param name="value">Desired value.</param>
        /// <param name="errorMessage">Error message when value is not assigned.</param>
        /// <returns>True = value is changed, False = variable's last value is retained.</returns>
        /// <exception cref="Exception"></exception>
        internal static bool AssignPrimitiveByte(this BasePrimitive variable, sbyte value, out string errorMessage)
        {
            if (variable is ByteVariable sb)
            {
                errorMessage = string.Empty;

                if (sb.Value != value)
                {
                    sb.Value = value;
                    return true;
                }

                return false;
            }
            else if (variable is WordVariable sw)
            {
                errorMessage = string.Empty;

                if (sw.Value != value)
                {
                    sw.Value = value;
                    return true;
                }

                return false;
            }
            else if (variable is DWordVariable sdw)
            {
                errorMessage = string.Empty;

                if (sdw.Value != value)
                {
                    sdw.Value = value;
                    return true;
                }

                return false;
            }
            else if (variable is QWordVariable sqw)
            {
                errorMessage = string.Empty;

                if (sqw.Value != value)
                {
                    sqw.Value = value;
                    return true;
                }

                return false;
            }
            else
            {
                errorMessage =
                    $"Byte value cannot be assigned to a variable " +
                    $"of type {variable.PrimitiveType}";

                return false;
            }
        }

        /// <summary>
        /// Assigns variable with given value.
        /// </summary>
        /// <param name="variable">The variable to be assigned.</param>
        /// <param name="value">Desired value.</param>
        /// <param name="errorMessage">Error message when value is not assigned.</param>
        /// <returns>True = value is changed, False = variable's last value is retained.</returns>
        /// <exception cref="Exception"></exception>
        internal static bool AssignPrimitiveWord(this BasePrimitive variable, short value, out string errorMessage)
        {
            if (variable is WordVariable sw)
            {
                errorMessage = string.Empty;

                if (sw.Value != value)
                {
                    sw.Value = value;
                    return true;
                }

                return false;
            }
            else if (variable is DWordVariable sdw)
            {
                errorMessage = string.Empty;

                if (sdw.Value != value)
                {
                    sdw.Value = value;
                    return true;
                }

                return false;
            }
            else if (variable is QWordVariable sqw)
            {
                errorMessage = string.Empty;

                if (sqw.Value != value)
                {
                    sqw.Value = value;
                    return true;
                }

                return false;
            }
            else if (variable is ByteVariable sb)
            {
                errorMessage = string.Empty;

                if (sb.Value != value)
                {
                    sb.Value = (sbyte)value;
                    return true;
                }

                return false;
            }
            else
            {
                errorMessage =
                    $"Word value cannot be assigned to a variable " +
                    $"of type {variable.PrimitiveType}";

                return false;
            }
        }

        /// <summary>
        /// Assigns variable with given value.
        /// </summary>
        /// <param name="variable">The variable to be assigned.</param>
        /// <param name="value">Desired value.</param>
        /// <param name="errorMessage">Error message when value is not assigned.</param>
        /// <returns>True = value is changed, False = variable's last value is retained.</returns>
        /// <exception cref="Exception"></exception>
        internal static bool AssignPrimitiveDWord(this BasePrimitive variable, int value, out string errorMessage)
        {
            if (variable is DWordVariable sdw)
            {
                errorMessage = string.Empty;

                if (sdw.Value != value)
                {
                    sdw.Value = value;
                    return true;
                }

                return false;
            }
            else if (variable is QWordVariable sqw)
            {
                errorMessage = string.Empty;

                if (sqw.Value != value)
                {
                    sqw.Value = value;
                    return true;
                }

                return false;
            }
            else if (variable is WordVariable sw)
            {
                errorMessage = string.Empty;

                if (sw.Value != value)
                {
                    sw.Value = (short)value;
                    return true;
                }

                return false;
            }
            else if (variable is ByteVariable sb)
            {
                errorMessage = string.Empty;

                if (sb.Value != value)
                {
                    sb.Value = (sbyte)value;
                    return true;
                }

                return false;
            }
            else
            {
                errorMessage =
                    $"DWord value cannot be assigned to a variable " +
                    $"of type {variable.PrimitiveType}";

                return false;
            }
        }

        /// <summary>
        /// Assigns variable with given value.
        /// </summary>
        /// <param name="variable">The variable to be assigned.</param>
        /// <param name="value">Desired value.</param>
        /// <param name="errorMessage">Error message when value is not assigned.</param>
        /// <returns>True = value is changed, False = variable's last value is retained.</returns>
        /// <exception cref="Exception"></exception>
        internal static bool AssignPrimitiveQWord(this BasePrimitive variable, long value, out string errorMessage)
        {
            if (variable is QWordVariable sqw)
            {
                errorMessage = string.Empty;

                if (sqw.Value != value)
                {
                    sqw.Value = value;
                    return true;
                }

                return false;
            }
            else if (variable is DWordVariable sdw)
            {
                errorMessage = string.Empty;

                if (sdw.Value != value)
                {
                    sdw.Value = (int)value;
                    return true;
                }

                return false;
            }
            else if (variable is WordVariable sw)
            {
                errorMessage = string.Empty;

                if (sw.Value != value)
                {
                    sw.Value = (short)value;
                    return true;
                }

                return false;
            }
            else if (variable is ByteVariable sb)
            {
                errorMessage = string.Empty;

                if (sb.Value != value)
                {
                    sb.Value = (sbyte)value;
                    return true;
                }

                return false;
            }
            else
            {
                errorMessage =
                    $"QWord value cannot be assigned to a variable " +
                    $"of type {variable.PrimitiveType}";

                return false;
            }
        }

        /// <summary>
        /// Assigns variable with given value.
        /// </summary>
        /// <param name="variable">The variable to be assigned.</param>
        /// <param name="value">Desired value.</param>
        /// <param name="errorMessage">Error message when value is not assigned.</param>
        /// <returns>True = value is changed, False = variable's last value is retained.</returns>
        /// <exception cref="Exception"></exception>
        internal static bool AssignPrimitiveUnsignedByte(this BasePrimitive variable, byte value, out string errorMessage)
        {
            if (variable is UnsignedByteVariable usb)
            {
                errorMessage = string.Empty;

                if (usb.Value != value)
                {
                    usb.Value = value;
                    return true;
                }

                return false;
            }
            else if (variable is UnsignedWordVariable usw)
            {
                errorMessage = string.Empty;

                if (usw.Value != value)
                {
                    usw.Value = value;
                    return true;
                }

                return false;
            }
            else if (variable is UnsignedDWordVariable usdw)
            {
                errorMessage = string.Empty;

                if (usdw.Value != value)
                {
                    usdw.Value = value;
                    return true;
                }

                return false;
            }
            else if (variable is UnsignedQWordVariable usqw)
            {
                errorMessage = string.Empty;

                if (usqw.Value != value)
                {
                    usqw.Value = value;
                    return true;
                }

                return false;
            }
            else
            {
                errorMessage =
                    $"UnsignedByte value cannot be assigned to a variable " +
                    $"of type {variable.PrimitiveType}";

                return false;
            }
        }

        /// <summary>
        /// Assigns variable with given value.
        /// </summary>
        /// <param name="variable">The variable to be assigned.</param>
        /// <param name="value">Desired value.</param>
        /// <param name="errorMessage">Error message when value is not assigned.</param>
        /// <returns>True = value is changed, False = variable's last value is retained.</returns>
        /// <exception cref="Exception"></exception>
        internal static bool AssignPrimitiveUnsignedWord(this BasePrimitive variable, ushort value, out string errorMessage)
        {
            if (variable is UnsignedWordVariable usw)
            {
                errorMessage = string.Empty;

                if (usw.Value != value)
                {
                    usw.Value = value;
                    return true;
                }

                return false;
            }
            else if (variable is UnsignedDWordVariable usdw)
            {
                errorMessage = string.Empty;

                if (usdw.Value != value)
                {
                    usdw.Value = value;
                    return true;
                }

                return false;
            }
            else if (variable is UnsignedQWordVariable usqw)
            {
                errorMessage = string.Empty;

                if (usqw.Value != value)
                {
                    usqw.Value = value;
                    return true;
                }

                return false;
            }
            else if (variable is UnsignedByteVariable usb)
            {
                errorMessage = string.Empty;

                if (usb.Value != value)
                {
                    usb.Value = (byte)value;
                    return true;
                }

                return false;
            }
            else
            {
                errorMessage =
                    $"UnsignedWord value cannot be assigned to a variable " +
                    $"of type {variable.PrimitiveType}";

                return false;
            }
        }

        /// <summary>
        /// Assigns variable with given value.
        /// </summary>
        /// <param name="variable">The variable to be assigned.</param>
        /// <param name="value">Desired value.</param>
        /// <param name="errorMessage">Error message when value is not assigned.</param>
        /// <returns>True = value is changed, False = variable's last value is retained.</returns>
        /// <exception cref="Exception"></exception>
        internal static bool AssignPrimitiveUnsignedDWord(this BasePrimitive variable, uint value, out string errorMessage)
        {
            if (variable is UnsignedDWordVariable usdw)
            {
                errorMessage = string.Empty;

                if (usdw.Value != value)
                {
                    usdw.Value = value;
                    return true;
                }

                return false;
            }
            else if (variable is UnsignedQWordVariable usqw)
            {
                errorMessage = string.Empty;

                if (usqw.Value != value)
                {
                    usqw.Value = value;
                    return true;
                }

                return false;
            }
            else if (variable is UnsignedWordVariable usw)
            {
                errorMessage = string.Empty;

                if (usw.Value != value)
                {
                    usw.Value = (ushort)value;
                    return true;
                }

                return false;
            }
            else if (variable is UnsignedByteVariable usb)
            {
                errorMessage = string.Empty;

                if (usb.Value != value)
                {
                    usb.Value = (byte)value;
                    return true;
                }

                return false;
            }
            else
            {
                errorMessage =
                    $"UnsignedDWord value cannot be assigned to a variable " +
                    $"of type {variable.PrimitiveType}";

                return false;
            }
        }

        /// <summary>
        /// Assigns variable with given value.
        /// </summary>
        /// <param name="variable">The variable to be assigned.</param>
        /// <param name="value">Desired value.</param>
        /// <param name="errorMessage">Error message when value is not assigned.</param>
        /// <returns>True = value is changed, False = variable's last value is retained.</returns>
        /// <exception cref="Exception"></exception>
        internal static bool AssignPrimitiveUnsignedQWord(this BasePrimitive variable, ulong value, out string errorMessage)
        {
            if (variable is UnsignedQWordVariable usqw)
            {
                errorMessage = string.Empty;

                if (usqw.Value != value)
                {
                    usqw.Value = value;
                    return true;
                }

                return false;
            }
            else if (variable is UnsignedDWordVariable usdw)
            {
                errorMessage = string.Empty;

                if (usdw.Value != value)
                {
                    usdw.Value = (uint)value;
                    return true;
                }

                return false;
            }
            else if (variable is UnsignedWordVariable usw)
            {
                errorMessage = string.Empty;

                if (usw.Value != value)
                {
                    usw.Value = (ushort)value;
                    return true;
                }

                return false;
            }
            else if (variable is UnsignedByteVariable usb)
            {
                errorMessage = string.Empty;

                if (usb.Value != value)
                {
                    usb.Value = (byte)value;
                    return true;
                }

                return false;
            }
            else
            {
                errorMessage =
                    $"UnsignedQWord value cannot be assigned to a variable " +
                    $"of type {variable.PrimitiveType}";

                return false;
            }
        }

        /// <summary>
        /// Assigns variable with given value.
        /// </summary>
        /// <param name="variable">The variable to be assigned.</param>
        /// <param name="value">Desired value.</param>
        /// <param name="errorMessage">Error message when value is not assigned.</param>
        /// <returns>True = value is changed, False = variable's last value is retained.</returns>
        /// <exception cref="Exception"></exception>
        internal static bool AssignPrimitiveSingle(this BasePrimitive variable, float value, out string errorMessage)
        {
            if (variable is SingleVariable sv)
            {
                errorMessage = string.Empty;

                if (sv.Value != value)
                {
                    sv.Value = value;
                    return true;
                }

                return false;
            }
            else if (variable is DoubleVariable dv)
            {
                errorMessage = string.Empty;

                if (dv.Value != value)
                {
                    dv.Value = value;
                    return true;
                }

                return false;
            }
            else
            {
                errorMessage =
                    $"Single value cannot be assigned to a variable " +
                    $"of type {variable.PrimitiveType}";

                return false;
            }
        }

        /// <summary>
        /// Assigns variable with given value.
        /// </summary>
        /// <param name="variable">The variable to be assigned.</param>
        /// <param name="value">Desired value.</param>
        /// <param name="errorMessage">Error message when value is not assigned.</param>
        /// <returns>True = value is changed, False = variable's last value is retained.</returns>
        /// <exception cref="Exception"></exception>
        internal static bool AssignPrimitiveDouble(this BasePrimitive variable, double value, out string errorMessage)
        {
            if (variable is DoubleVariable dv)
            {
                errorMessage = string.Empty;

                if (dv.Value != value)
                {
                    dv.Value = value;
                    return true;
                }

                return false;
            }
            else if (variable is SingleVariable sv)
            {
                errorMessage = string.Empty;

                if (sv.Value != value)
                {
                    sv.Value = (float)value;
                    return true;
                }

                return false;
            }
            else
            {
                errorMessage =
                    $"Double value cannot be assigned to a variable " +
                    $"of type {variable.PrimitiveType}";

                return false;
            }
        }
    }
}
