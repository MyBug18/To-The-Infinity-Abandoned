using System;
using System.Collections;
using System.Collections.Generic;

public class POP
{
    public string name { get; private set; }

    public Job aptitude { get; private set; }

    public bool isTraining { get; private set; } = false;
    //public Job currentJob { get; private set; } = Job.None;
    //public Job trainingJob { get; private set; } = Job.None;

    public (POPWorkingPlace workingPlace, int slotNum) currentWorkingPlace { get; private set; } = (null, 0); // Current POP's working place.
    public (POPWorkingPlace workingPlace, int slotNum) futureWorkingPlace { get; set; } = (null, 0); // After training ended, POP will move to this place.

    public float happiness => _basicHappiness + _jobHappiness;

    private float _basicHappiness => 50;

    private float _jobHappiness { get {
            if (currentWorkingPlace.workingPlace == null) return -20;
            else switch(_IsAptitudeMatching(currentWorkingPlace.workingPlace.GetJobOfWorkingSlot(currentWorkingPlace.slotNum)))
                {
                    case -1: return -10;
                    case 0: return 0;
                    case 1: return 10;
                    default: throw new InvalidOperationException("ERROR: Invalid Job Code");
                }
        }
    }


    public Planet planet; // The planet which this pop lives on.

    public int remainTrainingDay { get; private set; } = 0;

    public void DecreaseTrainingDay()
    {
        remainTrainingDay--;
    }
    
    public void StartTraining() // Removes current job, sets future job, and start training.
    {
        POPWorkingPlace goal = futureWorkingPlace.workingPlace;
        int slotnum = futureWorkingPlace.slotNum;

        isTraining = true;
        currentWorkingPlace = (null, 0);
        futureWorkingPlace = (goal, slotnum);
        switch(_IsAptitudeMatching(goal.GetJobOfWorkingSlot(slotnum)))
        {
            case 1:
                remainTrainingDay = 30;
                break;
            case 0:
                remainTrainingDay = 90;
                break;
            case -1:
                remainTrainingDay = 180;
                break;
        }

        Game.AddTrainingPOP(this); // Add POP to the training queue.
    }

    public void MoveJob() // After the training ended, changes it's current job, and allocates this POP to designated workingplace slot. Will be automatically called by Game._ProceedTraining().
    {
        isTraining = false;
        currentWorkingPlace = futureWorkingPlace;
        futureWorkingPlace = (null, 0);
        currentWorkingPlace.workingPlace.AllocatePOP(this, currentWorkingPlace.slotNum);
    }

    private int _IsAptitudeMatching(Job test) // 1 if perfectly matches. 0 if they are in same JobType. -1 if doesn't matches.
    {
        if (aptitude == test)
            return 1;
        else if (GetJobTypeOfJob(test) == GetJobTypeOfJob(aptitude))
            return 0;
        else
            return -1;
    }
    
    public static JobType GetJobTypeOfJob(Job job)
    {
        switch(job)
        {
            case Job.Technician:
            case Job.Miner:
            case Job.Farmer:
                return JobType.Worker;
            case Job.Physicist:
            case Job.Sociologist:
            case Job.Engineer:
                return JobType.Researcher;
            case Job.Clerk:
            case Job.Administrator:
            case Job.Enforcer:
                return JobType.PublicServant;
            case Job.Soldier:
            case Job.Admiral:
            case Job.Staff:
                return JobType.MilitaryPersonnel;
            default:
                throw new InvalidOperationException("ERROR: Invalid Job Detected: " + job);
        }

    }

}

public enum Job
{
    None,

    Technician,
    Miner,
    Farmer,

    Physicist,
    Sociologist,
    Engineer,

    Clerk,
    Administrator,
    Enforcer,

    Soldier,
    Staff,
    Admiral
}

public enum JobType
{
    Researcher , PublicServant, Worker, MilitaryPersonnel
}
