using System;
using System.Collections.Generic;
using System.Linq;

public static class BattleHelper
{
    public static (float distance, int direction) GetDistanceDirection(Unit a, Unit b)
    {
        var aPos = a.transform.position.x;
        var bPos = b.transform.position.x;

        if (aPos > bPos)
            return (aPos - bPos, -1);
        else if (aPos < bPos)
            return (bPos - aPos, 1);
        else
            return (0, 0);
    }

    public static bool IsUnitInDistance(Unit unit, Unit other, float distance)
    {
        return GetDistanceDirection(unit, other).distance <= distance;
    }

    public static (Unit, float, int) FindClosestHumanUnit(Unit unit, BattleContext context)
    {
        (Unit unit, float distance, int direction) result = new ValueTuple<Unit, float, int>(null, float.MaxValue, 0);

        foreach (var other in context.AllUnits.Where(x => x != null && !x.isAI))
        {
            var (distance, direction) = GetDistanceDirection(unit, other);
            if (distance < result.distance)
            {
                result.distance = distance;
                result.unit = other;
                result.direction = direction;
            }
        }

        return result;
    }
}