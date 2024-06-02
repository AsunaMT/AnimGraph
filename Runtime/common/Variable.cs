using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AnimGraph
{

    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public class Value
    {
        [FieldOffset(0)][SerializeField]
        private bool boolVal_;

        [FieldOffset(0)][SerializeField]
        private int intVal_;

        [FieldOffset(0)][SerializeField]
        private float floatVal_;

        [FieldOffset(8)]
        [SerializeField]
        private string stringVal_;

        public Value(bool boolVal)
        {
            boolVal_ = boolVal;
        }

        public Value(int intVal)
        {
            intVal_ = intVal;
        }

        public Value(float floatVal)
        {
            floatVal_ = floatVal;
        }

        public Value(string stringVal)
        {
            stringVal_ = stringVal;
        }

        public bool Bool()
        {
            return boolVal_;
        }

        public int Int()
        {
            return intVal_;
        }

        public float Float()
        {
            return floatVal_;
        }

        public string String()
        {
            return stringVal_;
        }

        public void SetBool(bool val)
        {
            boolVal_ = val;
        }

        public void SetInt(int val)
        {
            intVal_ = val;
        }

        public void SetFloat(float val)
        {
            floatVal_ = val;
        }

        public void SetString(string val)
        {
            stringVal_ = val;
        }
    }


    [Serializable]
    public enum VarType
    {
        EBool,
        EInt,
        EFloat,
        EString,
    }


    [Serializable]
    public class Variable
    {
        public VarType type;

        public string name;

        [SerializeReference]
        public Value value;

        public Variable(string name, bool value)
        {
            this.type = VarType.EBool;
            this.name = name;
            this.value = new Value(value);
        }

        public Variable(string name, int value)
        {
            this.type = VarType.EInt;
            this.name = name;
            this.value = new Value(value);
        }
        public Variable(string name, float value)
        {
            this.type = VarType.EFloat;
            this.name = name;
            this.value = new Value(value);
        }

        public bool GetBool()
        {
            return value.Bool();
        }

        public int GetInt()
        {
            return value.Int();
        }

        public float GetFloat() 
        { 
            return value.Float();
        }

        public void SetBool(bool val)
        {
            value.SetBool(val);
        }

        public void SetInt(int val)
        {
            value.SetInt(val);
        }

        public void SetFloat(float val)
        {
            value.SetFloat(val);
        }
    }
}
