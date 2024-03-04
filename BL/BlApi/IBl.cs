﻿using BlImplementation;

namespace BlApi;

public interface IBl
{
    public ITask Task { get; }
    public IEngineer Engineer { get; }
    public IMilestone Milestone { get; }
    public IDatesControl DateControl { get; }
    public IUser User { get; }
    public void InitializeDataBase();
    /// <summary>
    /// Resets all data in the system (tasks, engineers, dependencies, dates)
    /// </summary>
    public void Reset();
}
