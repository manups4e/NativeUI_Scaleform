using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.FiveM.GUI;
using ScaleformUI.Elements;
using System.Drawing;
using static CitizenFX.FiveM.Native.Natives;

namespace ScaleformUI.Scaleforms
{
    public class ScaleformWideScreen : IDisposable
    {
        public ScaleformWideScreen(string scaleformID)
        {
            _handle = RequestScaleformMovieInstance(scaleformID);
        }

        ~ScaleformWideScreen()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (IsLoaded)
            {
                SetScaleformMovieAsNoLongerNeeded(ref _handle);
            }

            GC.SuppressFinalize(this);
        }


        public int Handle
        {
            get { return _handle; }
        }

        private int _handle;

        public bool IsValid
        {
            get
            {
                return Handle != 0;
            }
        }
        public bool IsLoaded
        {
            get
            {
                return HasScaleformMovieLoaded(Handle);
            }
        }

        public void CallFunction(string function, params object[] arguments)
        {
            BeginScaleformMovieMethod(Handle, function);
            foreach (object argument in arguments)
            {
                switch (argument)
                {
                    case int argInt:
                        PushScaleformMovieMethodParameterInt(argInt);
                        break;
                    case string:
                    case char:
                        PushScaleformMovieMethodParameterString(argument.ToString());
                        break;
                    case double:
                    case float:
                        PushScaleformMovieMethodParameterFloat((float)argument);
                        break;
                    case bool argBool:
                        PushScaleformMovieMethodParameterBool(argBool);
                        break;
                    case ScaleformLabel argLabel:
                        BeginTextCommandScaleformString(argLabel.Label);
                        EndTextCommandScaleformString();
                        break;
                    case ScaleformLiteralString argLiteral:
                        ScaleformMovieMethodAddParamTextureNameString_2(argLiteral.LiteralString);
                        break;
                    case SColor color:
                        if (color == default)
                            PushScaleformMovieMethodParameterInt(SColor.HUD_None.ArgbValue);
                        else
                            PushScaleformMovieMethodParameterInt(color.ArgbValue);
                        break;
                    default:
                        throw new ArgumentException(string.Format("Unknown argument type '{0}' passed to scaleform with handle {1}...", argument.GetType().Name, Handle), "arguments");
                }
            }
            EndScaleformMovieMethod();
        }

        private int CallFunctionReturnInternal(string function, params object[] arguments)
        {
            BeginScaleformMovieMethod(Handle, function);
            foreach (object argument in arguments)
            {
                switch (argument)
                {
                    case int argInt:
                        PushScaleformMovieMethodParameterInt(argInt);
                        break;
                    case string:
                    case char:
                        PushScaleformMovieMethodParameterString(argument.ToString());
                        break;
                    case double:
                    case float:
                        PushScaleformMovieMethodParameterFloat((float)argument);
                        break;
                    case bool argBool:
                        PushScaleformMovieMethodParameterBool(argBool);
                        break;
                    case ScaleformLabel argLabel:
                        BeginTextCommandScaleformString(argLabel.Label);
                        EndTextCommandScaleformString();
                        break;
                    case ScaleformLiteralString argLiteral:
                        ScaleformMovieMethodAddParamTextureNameString_2(argLiteral.LiteralString);
                        break;
                    default:
                        throw new ArgumentException(string.Format("Unknown argument type '{0}' passed to scaleform with handle {1}...", argument.GetType().Name, Handle), "arguments");
                }
            }
            return EndScaleformMovieMethodReturnValue();
        }
        public async Task<int> CallFunctionReturnValueInt(string function, params object[] arguments)
        {
            int ret = CallFunctionReturnInternal(function, arguments);
            while (!IsScaleformMovieMethodReturnValueReady(ret)) await BaseScript.Delay(0);
            return GetScaleformMovieFunctionReturnInt(ret);
        }
        public async Task<bool> CallFunctionReturnValueBool(string function, params object[] arguments)
        {
            int ret = CallFunctionReturnInternal(function, arguments);
            while (!IsScaleformMovieMethodReturnValueReady(ret)) await BaseScript.Delay(0);
            return GetScaleformMovieMethodReturnValueBool(ret);
        }
        public async Task<string> CallFunctionReturnValueString(string function, params object[] arguments)
        {
            int ret = CallFunctionReturnInternal(function, arguments);
            while (!IsScaleformMovieMethodReturnValueReady(ret)) await BaseScript.Delay(0);
            return GetScaleformMovieFunctionReturnString(ret);
        }

        public void Render2D()
        {
            DrawScaleformMovieFullscreen(Handle, 255, 255, 255, 255, 0);
        }
        public void Render2DScreenSpace(PointF location, PointF size)
        {
            float x = location.X / Screen.Width;
            float y = location.Y / Screen.Height;
            float width = size.X / Screen.Width;
            float height = size.Y / Screen.Height;

            DrawScaleformMovie(Handle, x + (width / 2.0f), y + (height / 2.0f), width, height, 255, 255, 255, 255, 0);
        }
        public void Render3D(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            DrawScaleformMovie_3dNonAdditive(Handle, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 2.0f, 2.0f, 1.0f, scale.X, scale.Y, scale.Z, 2);
        }
        public void Render3DAdditive(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            DrawScaleformMovie_3d(Handle, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 2.0f, 2.0f, 1.0f, scale.X, scale.Y, scale.Z, 2);
        }
    }

    public static class TypeCache<T>
    {
        static TypeCache()
        {
            Type = typeof(T);
            IsSimpleType = true;
            switch (Type.GetTypeCode(Type))
            {
                case TypeCode.Object:
                case TypeCode.DBNull:
                case TypeCode.Empty:
                case TypeCode.DateTime:
                    IsSimpleType = false;
                    break;
            }
        }

        public static bool IsSimpleType { get; }
        public static Type Type { get; }
    }
}
