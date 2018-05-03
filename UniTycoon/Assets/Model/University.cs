using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameDifficulty {Easy,Normal,Hard}
public enum UniversityLevel {BreakingGround,InitialExpansion,Stage1,Stage2,Stage3,Victory,Loss}

public class University
{
//CLASS VARS

	GameDifficulty difficulty;//----game difficulty, adjusts most parameters
	UniversityLevel level;//--------stage in game progression

	int residentCapacity;//----------total capacity for residents
	double residentPopulation;//-----population of rent payers
	int studentCapacity;//-----------total capacity for students
	double studentPopulation;//------population of tuition payers
	double studentGrowthFactor;//---intermediate variable for logistic function
	double studentGrowthRate;//-----max percentage growth rate for students
	double residentGrowthFactor;//--intermediate variable for logistic function
	double residentGrowthRate;//----max percentage growth rate for residents

	double coffers;//---------------total player money in bank/wallet basic score
	double tuitionRate;//-----------tuition charged per student per ingame year
	double rentRate;//--------------rent charged per resident per ingame month
	double scholarshipAllocation;//-money allocated to scholarships (deducted from tuition revenue)
	double grantAllocation;//-------money allocated to research grants (deducted from tuition revenue)
    double studentUpkeep;//---------cost of paying for academic buildings/staff/students
    double residentUpkeep;//--------cost of upkeep for residential buildings

	double EASY_MOD;
	double NORM_MOD;
	double HARD_MOD;
	int MONTH;
	int YEAR;
    double DAY;
    double HOUR;
    long time;

//CONSTRUCTOR

	public University ()
	{
        difficulty = GameDifficulty.Normal;
		level = UniversityLevel.BreakingGround;
		EASY_MOD = 1.5;
		NORM_MOD = 1.0;
		HARD_MOD = 0.6;
        HOUR = 0.25;
        DAY = 24 * HOUR;
        MONTH = (int)(30 * DAY);
		YEAR = 12 * MONTH;

		double START_COFFERS = 150000.00;
		switch(difficulty)
		{
		case GameDifficulty.Easy:
			coffers = START_COFFERS * EASY_MOD;
			break;
		case GameDifficulty.Normal:
			coffers = START_COFFERS * NORM_MOD;
			break;
		case GameDifficulty.Hard:
			coffers = START_COFFERS * HARD_MOD;
			break;
		}

		residentCapacity = 0;
		residentPopulation = 0;
		studentCapacity = 0;
		studentPopulation = 0;

		studentGrowthRate = 0.5; //Month
        studentGrowthFactor = 0;
		residentGrowthRate = 0.5; //Month
        residentGrowthFactor = 0;
		tuitionRate = 10000; //Year
		rentRate = 500; //Month
		scholarshipAllocation = 0; //Year
		grantAllocation = 0; //Year
        studentUpkeep = 5000; //Year
        residentUpkeep = 200; //Month 

        time = 0;
	}

//METHODS

	public void updateUniversityVars (World world)
	{
        clock();
		studentCapacity = world.TotalStudentCapacity;
		residentCapacity = world.TotalResidentCapacity;
        if (level != UniversityLevel.BreakingGround)
        {
            studentGrowthFactor = (studentGrowthRate / MONTH) * studentPopulation * ((studentCapacity - studentPopulation) / studentCapacity);
            studentPopulation = studentPopulation + studentGrowthFactor;
            residentGrowthFactor = (residentGrowthRate / MONTH) * residentPopulation * ((residentCapacity - residentPopulation) / residentCapacity);
            residentPopulation = residentPopulation + residentGrowthFactor;
            coffers = coffers + (((tuitionRate / YEAR) - (studentUpkeep / YEAR)) * studentPopulation) + (((rentRate / MONTH) - (residentUpkeep / MONTH)) * residentPopulation) - (scholarshipAllocation / YEAR) - (grantAllocation / YEAR);
        }
		switch(difficulty)
		{
		case GameDifficulty.Easy:
			break;
		case GameDifficulty.Normal:
			break;
		case GameDifficulty.Hard:
			break;
		}
        if (coffers < 0)
            lose();
	}

	public string HUD()
	{
        int studentPop = (int)studentPopulation;
        int residentPop = (int)residentPopulation;
        return "Time: " + Time() + "\nLevel: " + LevelToString() + "\nMoney: $" + Coffers.ToString() + "\nStudents (POP/CAP): " + studentPop.ToString() + "/" + studentCapacity.ToString() + "\nResidents (POP/CAP): " + residentPop.ToString() + "/" + residentCapacity.ToString();
	}

    public string budget(){
        return "Budget:\nTotal money: "+coffers+"\nTotal costs (per clock): "+(int)((studentUpkeep / YEAR) + (residentUpkeep / MONTH) + (scholarshipAllocation / YEAR) + (grantAllocation / YEAR))+"\nStudent upkeep (yearly): "+studentUpkeep+"\nResident upkeep (monthly)"+residentUpkeep+"\nScholarship allocation (yearly): "+scholarshipAllocation+"\nGrant allocation (yearly): "+grantAllocation+"\n\nTotal revenue (per clock): "+(int)((tuitionRate / YEAR)+(rentRate / MONTH))+"\nTuition (yearly): "+tuitionRate+"\nRent (monthly): "+rentRate;
    }

    private void clock(){
        time++;
    }
    public string LevelToString(){
        switch(level)
        {
            case UniversityLevel.BreakingGround:
                return "Breaking Ground!";
            case UniversityLevel.InitialExpansion:
                return "Initial Expansion";
            case UniversityLevel.Stage1:
                return "Stage 1";
            case UniversityLevel.Stage2:
                return "Stage 2";
            case UniversityLevel.Stage3:
                return "Stage 3";
            case UniversityLevel.Victory:
                return "Victory!";
            case UniversityLevel.Loss:
                return "Loss!";
        }
        return "N/A";
    }
    private string Time(){
        long tmp = time;
        int years = (int)(tmp / YEAR);
        tmp = tmp - years * YEAR;
        int months = (int)(tmp / MONTH);
        tmp = tmp - months * MONTH;
        int days = (int)(tmp / DAY);
        tmp = (long)((double)tmp - days * DAY);
        int hours = (int)(tmp / HOUR);
        return "Y.M.D.H: " + years.ToString() + "." + months.ToString() + "." + days.ToString() + "." + hours.ToString();
    }
    
    public void advanceLevel(World world)
    {
        switch(level)
        {
            case UniversityLevel.BreakingGround:
                if(initialExpansionReqsMet(world)){
                    level = UniversityLevel.InitialExpansion;
                    studentPopulation = 1;
                    residentPopulation = 1;
                    //advance
                }else{
                    Debug.Log("Requirements to advance not yet met");
                }break;
            case UniversityLevel.InitialExpansion:
                if(Stage1ReqsMet(world)){
                    level = UniversityLevel.Stage1;
                    //advance
                }else{
                    Debug.Log("Requirements to advance not yet met");
                }break;
            case UniversityLevel.Stage1:
                if(Stage2ReqsMet(world)){
                    level = UniversityLevel.Stage2;
                    //advance
                }else{
                    Debug.Log("Requirements to advance not yet met");
                }break;
            case UniversityLevel.Stage2:
                if(Stage3ReqsMet(world)){
                    level = UniversityLevel.Stage3;
                    //advance
                }else{
                    Debug.Log("Requirements to advance not yet met");
                }break;
            case UniversityLevel.Stage3:
                if(VictoryReqsMet(world)){
                    level = UniversityLevel.Victory;
                    //advance
                } else{
                    Debug.Log("Requirements to advance not yet met");
                }break;
        }
    }   
    public bool canAdvanceLevel(World world)
    {
        switch (level)
        {
            case UniversityLevel.BreakingGround:
                return initialExpansionReqsMet(world);
            case UniversityLevel.InitialExpansion:
                return Stage1ReqsMet(world);
            case UniversityLevel.Stage1:
                return Stage2ReqsMet(world);
            case UniversityLevel.Stage2:
                return Stage3ReqsMet(world);
            case UniversityLevel.Stage3:
                return VictoryReqsMet(world);
        }
        return false;
    }
    private bool initialExpansionReqsMet(World world)
    {
        Tile tile_sr;
        bool hasAdminBldg = false;
        int totalStudentCap = world.TotalStudentCapacity;
        int totalResidentCap = world.TotalResidentCapacity;
        for (int i = 0; i < world.Width; i++){
            for (int j = 0; j < world.Height; j++){
                tile_sr = world.GetTileAt(i, j);
                if(tile_sr.Type == Tile.TileType.Admin){
                    hasAdminBldg = true;
                }
            }
        }
        return (hasAdminBldg && (totalStudentCap >= 1000) && (totalResidentCap >= 500));
    }
    private bool Stage1ReqsMet(World world)
    {
        bool hasLib = false, hasGym = false;
        Tile tile_sr;
        int totalStudentCap = world.TotalStudentCapacity;
        int totalResidentCap = world.TotalResidentCapacity;
        for (int i = 0; i < world.Width; i++)
        {
            for (int j = 0; j < world.Height; j++)
            {
                tile_sr = world.GetTileAt(i, j);
                if (tile_sr.Type == Tile.TileType.Gym)
                    hasGym = true;
                if (tile_sr.Type == Tile.TileType.Library)
                    hasLib = true;
            }
        }
        return (hasLib && hasGym && (totalStudentCap >= 5000) && (totalResidentCap >= 1000));
    }
    private bool Stage2ReqsMet(World world)
    {
        bool hasCafe = false, hasParking = false;
        Tile tile_sr;
        for (int i = 0; i < world.Width; i++)
        {
            for (int j = 0; j < world.Height; j++)
            {
                tile_sr = world.GetTileAt(i, j);
                if (tile_sr.Type == Tile.TileType.Cafe)
                    hasCafe = true;
                if (tile_sr.Type == Tile.TileType.Parking)
                    hasParking = true;
            }
        }
        return (hasCafe && hasParking && (studentPopulation >= 5000) && (residentPopulation >= 1000));
    }
    private bool Stage3ReqsMet(World world)
    {
        Tile tile_sr;
        bool hasStadium = false;
        for (int i = 0; i < world.Width; i++)
        {
            for (int j = 0; j < world.Height; j++)
            {
                tile_sr = world.GetTileAt(i, j);
                if (tile_sr.Type == Tile.TileType.Stadium)
                    hasStadium = true;
            }
        }
        return (hasStadium && (studentPopulation >= 10000) && (residentPopulation >= 3000) && (coffers >= 150000));
    }
    private bool VictoryReqsMet(World world){
        Tile tile_sr;
        bool hasLab = false;
        for (int i = 0; i < world.Width; i++)
        {
            for (int j = 0; j < world.Height; j++)
            {
                tile_sr = world.GetTileAt(i, j);
                if (tile_sr.Type == Tile.TileType.Lab)
                    hasLab = true;
            }
        }
        return (hasLab && (studentPopulation >= 30000) && (residentPopulation >= 10000) && (coffers >= 1000000));
    }
    public void addScholarship(double val){ scholarshipAllocation += val; }
    public void addGrant(double val){ grantAllocation += val; }
    public void addStudents(int val){ studentPopulation += val; }
    public void addResidents(int val){ residentPopulation += val; }

    public bool breakingGround() { return level == UniversityLevel.BreakingGround; }
    public bool initialExpansion() { return level == UniversityLevel.InitialExpansion; }
    public bool stage1() { return level == UniversityLevel.Stage1; }
    public bool stage2() { return level == UniversityLevel.Stage2; }
    public bool stage3() { return level == UniversityLevel.Stage3; }
    public bool victory() { return level == UniversityLevel.Victory; }
    public bool loss() { return level == UniversityLevel.Loss; }

    public void activateTimeAcc(){
        HOUR = 0.25/100;
        DAY = 24 * HOUR;
        MONTH = (int)(30 * DAY);
        YEAR = 12 * MONTH;
    }
    public void deactivateTimeAcc(){
        HOUR = 0.25;
        DAY = 24 * HOUR;
        MONTH = (int)(30 * DAY);
        YEAR = 12 * MONTH;
    }
    public void advertise(int money){
        studentGrowthRate = studentGrowthRate * (1 + ((double)money / coffers));
        residentGrowthRate = residentGrowthRate * (1 + ((double)money / coffers));
        coffers = coffers - (double)money;
    }
    public void lose(){
        level = UniversityLevel.Loss;
    }


//MUTATORS

	public int Coffers{
		get{ 
			return (int)coffers;	
		}
        set{
            coffers = value;
        }
	}
    public double ScholarshipAllocation{
        get{
            return scholarshipAllocation;
        }
        set{
            scholarshipAllocation = value;
        }
    }
    public double GrantAllocation{
        get{
            return grantAllocation;
        }
        set{
            grantAllocation = value;
        }
    }
    public double TuitionRate{
        get{
            return tuitionRate;
        }
    }
    public double RentRate{
        get{
            return rentRate;
        }
    }
    public int StudentCapacity{
        get{
            return studentCapacity;
        }
        set{
            studentCapacity = value;
        }
    }
    public double StudentPopulation{
        get{
            return studentPopulation;
        }
        set{
            studentPopulation = value;
        }
    }
    public int ResidentCapacity{
        get{
            return residentCapacity;
        }
        set{
            residentCapacity = value;
        }
    }
    public double ResidentPopulation{
        get{
            return residentPopulation;
        }
        set{
            residentPopulation = value;
        }
    }
    public UniversityLevel Level{
        get{
            return level;
        }
        set{
            level = value;
        }
    }
}