using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Licence
{
    public string Name { get => licenceName; set => licenceName = value; }
    public string Description { get => description; }
    public int Tier { get => tier; set => tier = value; }
    public int[] Costs { get => costs; }
    public int MaximumPoints { get => maximumPoints; }
    public int PointsInvested { get => saveData.pointsInvested; set => saveData.pointsInvested = value; }
    public LicenceEffect Effect { get => effect; }
}   