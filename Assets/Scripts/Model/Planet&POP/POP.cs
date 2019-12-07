using System;

public class POP
{
    public string name { get; private set; }

    public Job aptitude { get; private set; }

    public bool isUnemployed => currentWorkingSlot == null;
    public bool isTraining { get; private set; } = false;

    public POPWorkingSlot currentWorkingSlot { get; private set; } // Current POP's working place.
    public POPWorkingSlot futureWorkingSlot { get; set; } // After training ended, POP will move to this place.

    public float happiness { get
        {
            float result = basicHappiness + jobHappinessModifier + resourceHappinessModifier + amenityHappinessModifier + housingHappinessModifier + additionalHappinessModifier;
            return Math.Min(Math.Max(result, 0), 100);
        }
    }

    public float basicHappiness => 50;

    public float jobHappinessModifier { get
        {
            if (currentWorkingSlot == null) return -20;
            else switch(_IsAptitudeMatching(currentWorkingSlot.job))
                {
                    case -1: return -10;
                    case 0: return 0;
                    case 1: return 10;
                    default: throw new InvalidOperationException("ERROR: Invalid Job Code");
                }
        }
    }

    public float resourceHappinessModifier { get
        {
            int _food = 0;
            int _money = 0;
            if (planet.game.globalResource.isLackOfFood) _food = -25;
            if (planet.game.globalResource.isLackOfMoney) _money = -40;
            return _food + _money;
        }
    }

    public float amenityHappinessModifier {  get
        {
            int popNumber = planet.pops.Count;

            if (popNumber == 0) return 0;

            return (float)planet.amenity / popNumber * 20;
        }
    }

    public float housingHappinessModifier { get
        {
            int popNumber = planet.pops.Count;

            if (popNumber == 0) return 0;

            return (float)Math.Min(planet.housing, 0) / popNumber * 20;
        }
    }

    public float additionalHappinessModifier { get; set; } = 0;

    public POP(string name, Planet_Inhabitable planet)
    {
        System.Random r = new System.Random();
        this.name = name;
        int aptitude = r.Next() % 12 + 1;
        this.aptitude = (Job)aptitude; // Sets aptitude.
        this.planet = planet;
    }    

    public Planet_Inhabitable planet { get; private set; }

    public int remainTrainingDay { get; private set; } = 0;

    public void DecreaseTrainingDay()
    {
        remainTrainingDay--;
    }
    
    public void ActivatePOP(POPWorkingSlot futureSlot) // Activate just created POP, which must be unemployed
    {
        if (futureSlot.pop != null)
            throw new InvalidOperationException("Trying to move to already occupied slot!");

        if (futureSlot.isPOPTrainingForHere)
            throw new InvalidOperationException("Someone is already training for there!");

        if (!isUnemployed) throw new InvalidOperationException("Trying to activate employed POP!");
        else
        {
            _StartTraining(futureSlot);
            planet.unemployedPOPs.Remove(this);
            UnityEngine.Debug.Log("Start Training.");
        }
    }
    public void MovePOPJob(POPWorkingSlot futureSlot)
    {
        futureWorkingSlot = futureSlot;

        if (futureWorkingSlot.pop != null)
            throw new InvalidOperationException("Trying to move to already occupied slot!");

        if (futureWorkingSlot.isPOPTrainingForHere)
            throw new InvalidOperationException("Someone is already training for there!");

        if (currentWorkingSlot.pop == null) throw new InvalidOperationException("Trying to move pop which doesn't exist!");

        foreach (var upkeep in currentWorkingSlot.upkeeps)
        {
            upkeep.pop = null;
            planet.planetJobUpkeeps.Remove(upkeep);
        }

        foreach (var yield in currentWorkingSlot.yields)
        {
            yield.pop = null;
            planet.planetJobYields.Remove(yield);
        }

        currentWorkingSlot.pop = null;

        switch (currentWorkingSlot.job)
        {
            case Job.Administrator:
                planet.stabilityModifier -= 5;
                planet.providedAmenity -= 5;
                break;
            case Job.Admiral:
                planet.game.fleetNum--;
                break;
            case Job.Enforcer:
                planet.crimeReducedByEnforcer -= 30;
                planet.stabilityModifier -= 2;
                break;
            case Job.Clerk:
                planet.providedAmenity -= 3;
                break;
            case Job.Staff:
                planet.game.fleetAttackModifier -= 0.05f;
                break;
            case Job.Soldier:
                planet.game.defencePlatformAttackModifier -= 0.05f;
                break;
        }

        _StartTraining(futureSlot);
    }

    private void _AllocatePOPToCurrentSlot() // After the POP's current working slot is designated, fully allocate POP to current working slot.
    {
        currentWorkingSlot.isPOPTrainingForHere = false;
        currentWorkingSlot.pop = this;

        foreach (var upkeep in currentWorkingSlot.upkeeps)
        {
            upkeep.pop = this;
            planet.planetJobUpkeeps.Add(upkeep);
        }

        foreach (var yield in currentWorkingSlot.yields)
        {
            yield.pop = this;
            planet.planetJobYields.Add(yield);
        }

        switch (currentWorkingSlot.job)
        {
            case Job.Administrator:
                planet.stabilityModifier += 5;
                planet.providedAmenity += 5;
                break;
            case Job.Admiral:
                planet.game.fleetNum++;
                break;
            case Job.Enforcer:
                planet.crimeReducedByEnforcer += 30;
                planet.stabilityModifier += 2;
                break;
            case Job.Clerk:
                planet.providedAmenity += 3;
                break;
            case Job.Staff:
                planet.game.fleetAttackModifier += 0.05f;
                break;
            case Job.Soldier:
                planet.game.defencePlatformAttackModifier += 0.05f;
                break;
        }

    }


    private void _StartTraining(POPWorkingSlot futureSlot) // Removes current job, sets future job, and start training.
    {
        if (isTraining) throw new InvalidOperationException("Trying to train POP already training!");

        futureSlot.isPOPTrainingForHere = true;

        futureWorkingSlot = futureSlot;

        isTraining = true;

        remainTrainingDay = _GetTrainingDay();

        currentWorkingSlot = null;

        planet.trainingPOPs.Add(this); // Add POP to the training queue.
    }

    public void EndTraining() // After the training ended, changes it's current job, and allocates this POP to designated workingplace slot. Will be automatically called by Game._ProceedTraining().
    {
        isTraining = false;
        currentWorkingSlot = futureWorkingSlot;
        futureWorkingSlot = null;
        _AllocatePOPToCurrentSlot();
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

    private int _GetTrainingDay()
    {
        int _result = 0;

        if (currentWorkingSlot == null)
        {
            switch(_IsAptitudeMatching(futureWorkingSlot.job))
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
            Job currentJob = currentWorkingSlot.job;
            Job futureJob = futureWorkingSlot.job;

            if (currentJob == futureJob)
                _result = 60;
            else if (GetJobTypeOfJob(currentJob) == GetJobTypeOfJob(futureJob))
                _result = 120;
            else
                _result = 180;

            switch (_IsAptitudeMatching(futureWorkingSlot.job))
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

        string currentJob = "";

        if (currentWorkingSlot == null) currentJob = "Not Employed";
        else currentJob = "" + currentWorkingSlot.job;

        result = name + ": " + aptitude + ", Happiness: " + happiness + ", Current Job: " + currentJob;

        string happinessModifiers = 
            "Basic: " + basicHappiness + ", Job: " + jobHappinessModifier + 
            ", Resources: " + resourceHappinessModifier + ", Amenity: " + amenityHappinessModifier + 
            ", Housing: " + housingHappinessModifier;
        return result + "\n" + happinessModifiers;
    }

    public static JobType GetJobTypeOfJob(Job job)
    {
        switch (job)
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
