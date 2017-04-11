using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using UnityEngine;

namespace DotFuzzy
{
    /// <summary>
    /// Represents a collection of membership functions.
    /// </summary>

    public class MembershipFunctionCollection : Collection<MembershipFunction>
    {
        /// <summary>
        /// Finds a membership function in a collection.
        /// </summary>

        public MembershipFunction Find(string membershipFunctionName)
        {
            MembershipFunction membershipFunction = null;

            foreach (MembershipFunction function in this)
            {
                if (function.Name == membershipFunctionName)
                {
                    membershipFunction = function;
                    break;
                }
            }

            if (membershipFunction == null)
                throw new Exception("MembershipFunction not found: " + membershipFunctionName);
            else
                return membershipFunction;
        }
    }
}
