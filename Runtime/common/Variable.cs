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
    public class Variable
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

        public Variable(bool boolVal)
        {
            boolVal_ = boolVal;
        }

        public Variable(int intVal)
        {
            intVal_ = intVal;
        }

        public Variable(float floatVal)
        {
            floatVal_ = floatVal;
        }

        public Variable(string stringVal)
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
}
