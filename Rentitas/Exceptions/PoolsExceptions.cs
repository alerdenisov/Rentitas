using System;

namespace Rentitas
{
    public class UnknownTypeOfPoolException : RentitasException {
        public UnknownTypeOfPoolException(Type type) 
            : base("Non registered or incorrect type is coming " + type, null)
        {
        }
    }

    public class NonRegisteredPoolsException : RentitasException
    {
        public NonRegisteredPoolsException(Type type)
            : base("It's correct type, but non one pool is registered yet " + type, null)
        {
        }
    }

    public class IncorrectIndexOfPool : RentitasException
    {
        public IncorrectIndexOfPool(int index) 
            : base("Incorrect index of pool inside repository " +index, null)
        {
        }
    }
}