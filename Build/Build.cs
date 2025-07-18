using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;

class Build : NukeBuild, IPublishPack, IAutoCADPackageBuilder, IPrePack
{
    string IHazPackageBuilderProject.Name => "Example";
    public static int Main() => Execute<Build>(x => x.From<IPublishPack>().Build);
}