using System;

using UnityEngine;

public class POP
{
    public string name { get; private set; }

    public Job aptitude { get; private set; }

    public bool isUnemployed { get; private set; } = true;
    public bool isTraining { get; private set; } = false;

    public (POPWorkingPlace workingPlace, int slotNum) currentWorkingPlace { get; private set; } // Current POP's working place.
    public (POPWorkingPlace workingPlace, int slotNum) futureWorkingPlace { get; set; } // After training ended, POP will move to this place.

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

    public POP(string name, Planet_Inhabitable planet)
    {
        System.Random r = new System.Random();
        this.name = name;
        int aptitude = r.Next() % 12 + 1;
        this.aptitude = (Job)aptitude; // Sets aptitude.
        this.planet = planet;

        Debug.Log(this);
    }    

    public Planet_Inhabitable planet { get; private set; }

    public int remainTrainingDay { get; private set; } = 0;


    public void DecreaseTrainingDay()
    {
        remainTrainingDay--;
    }
    
    public void ActivatePOP(POPWorkingPlace workingPlace, int slotNum) // Activate just created POP, which must be unemployed
    {
        if (workingPlace.workingPOPList[slotNum].pop != null)
            throw new InvalidOperationException("Trying to move to already occupied slot!");

        if (workingPlace.workingPOPList[slotNum].isPOPTrainingForHere)
            throw new InvalidOperationException("Someone is already training for there!");

        if (!isUnemployed) throw new InvalidOperationException("Trying to activate employed POP!");
        else
        {
            isUnemployed = false;
            StartTraining(workingPlace, slotNum);
            planet.unemployedPOPs.Remove(this);
        }
    }

    public void StartTraining(POPWorkingPlace workingPlace, int slotNum) // Removes current job, sets future job, and start training.
    {
        if (isTraining) throw new InvalidOperationException("Trying to train POP already training!");

        workingPlace.workingPOPList[slotNum].isPOPTrainingForHere = true;

        Debug.Log("Start Training: " + slotNum + "th slot of " + workingPlace.name); // mineral, 1

        futureWorkingPlace = (workingPlace, slotNum);

        isTraining = true;

        remainTrainingDay = _GetTrainingDay();
        Debug.Log("StartTraining: remainTrainingDay = " + remainTrainingDay);

        currentWorkingPlace = (null, 0);

        planet.trainingPOPs.Add(this); // Add POP to the training queue.
    }

    public void MoveJob() // After the training ended, changes it's current job, and allocates this POP to designated workingplace slot. Will be automatically called by Game._ProceedTraining().
    {
        Debug.Log("MoveJob(): Moving to " + futureWorkingPlace.slotNum + "th slot of " + futureWorkingPlace.workingPlace.name);

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

    private int _GetTrainingDay()
    {
        int _result = 0;

        var futurePlace = futureWorkingPlace.workingPlace;

        if (currentWorkingPlace.workingPlace == null)
        {
            switch(_IsAptitudeMatching(futurePlace.GetJobOfWorkingSlot(futureWorkingPlace.slotNum)))
            {
                case 1:
                    _result = 30;
                    break;
                case 0:
                    _result = 60;
                    break;
                case -1:
                    _result = 90;
                    break;
                default: throw new InvalidOperationException("ERROR: Invalid Job detected.");
            }
        }
        else
        {
            Job currentJob = currentWorkingPlace.workingPlace.GetJobOfWorkingSlot(currentWorkingPlace.slotNum);
            Job futureJob = futureWorkingPlace.workingPlace.GetJobOfWorkingSlot(futureWorkingPlace.slotNum);

            if (currentJob == futureJob)
                _result = 60;
            else if (GetJobTypeOfJob(currentJob) == GetJobTypeOfJob(futureJob))
                _result = 120;
            else
                _result = 180;

            switch (_IsAptitudeMatching(futurePlace.GetJobOfWorkingSlot(futureWorkingPlace.slotNum)))
            {
                case 1:
                    _result = _result / 3;
                    break;
                case 0:
                    _result = _result / 2;
                    break;
                case -1:
                    break;
                default: throw new InvalidOperationException("ERROR: Invalid Job detected.");
            }           
        }
        return _result / 10;
    }

    public override string ToString()
    {
        string result = "";

        string employed = "";
        string training = "";

        if (isUnemployed) employed = "unemployed ";

        if (isTraining)
            training = "training (" + remainTrainingDay + "days left) ";
        else
            training = "not training";
        result = name + ": " + aptitude + " " + employed + training;
        return result;
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
