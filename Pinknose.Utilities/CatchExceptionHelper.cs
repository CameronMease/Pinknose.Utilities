using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pinknose.Utilities
{
    public static class CatchExceptionHelper
    {
        public static void VerifyExceptionCaught<TException>(Action predicate) where TException : Exception
        {
            bool exceptionCaught = false;

            if (predicate == null)
            {
                Assert.Fail("Action was null.");
                return;
            }

            try
            {
                predicate();
            }
            catch (TException)
            {
                exceptionCaught = true;
            }

            Assert.IsTrue(exceptionCaught);
        }
    }
}
