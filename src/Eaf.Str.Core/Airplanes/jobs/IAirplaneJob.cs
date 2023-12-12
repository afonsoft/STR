using Eaf.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eaf.Str.Airplanes.Jobs
{
    public interface IAirplaneJob : ITransientDependency
    {
        Task StartProcess();
    }
}