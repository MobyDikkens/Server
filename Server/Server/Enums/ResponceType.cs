using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Enums
{
    public enum DMLResponce { UnknownPackage = 0, BadRequest, UserExists, UnknownUser, RegistrationOk, UserIsAlreadyExists, UserNotFound, Ok, FileNotFound };
}
