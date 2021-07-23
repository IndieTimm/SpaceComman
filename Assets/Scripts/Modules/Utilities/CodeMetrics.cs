using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CodeMetricsSessionMeasuring
{
    public string Name { get; }
    public DateTime Start { get; }
    public DateTime End { get; private set; }

    public CodeMetricsSessionMeasuring(string name)
    {
        Name = name;
        Start = DateTime.Now;
    }

    public void EndMetrics()
    {
        End = DateTime.Now;
    }
}

public class CodeMetricsSession
{
    public int ID { get; }

    private List<CodeMetricsSessionMeasuring> measurings = new List<CodeMetricsSessionMeasuring>();

    public CodeMetricsSession(int sessionNumber)
    {
        ID = sessionNumber;
    }

    public CodeMetricsSessionMeasuring BeginMeasuring(string name)
    {
        return new CodeMetricsSessionMeasuring(name);
    }

    public void EndMeasuring(CodeMetricsSessionMeasuring measuring)
    {
        measuring.EndMetrics();
        measurings.Add(measuring);
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();

        string header = $"[{ID}]=================";

        builder.AppendLine(header);

        double sum = measurings.Sum(x => (x.End - x.Start).TotalMilliseconds);

        builder.AppendLine(string.Join("\n", measurings.Select(x =>
            $"{x.Name} " + GetTimeMeasuring((x.End - x.Start).TotalMilliseconds, sum))));

        builder.AppendLine(new string('=', header.Length));

        return builder.ToString();
    }

    private string GetTimeMeasuring(double currentTime, double allTime)
    {
        return 
            $"duration:{currentTime:0.00}ms " +
            $"{((currentTime == 0) ? 0 : allTime / currentTime) * 100:0.00}%";
    }
}

public static class CodeMetrics
{
    private static int lastSession = 0;

    public static CodeMetricsSession StartSession()
    {
        CodeMetricsSession session = new CodeMetricsSession(lastSession);
        lastSession++;
        return session;
    }

    public static void EndSession(CodeMetricsSession session)
    {
        Debug.Log(session.ToString());
    }
}