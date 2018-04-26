using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum UniversitySize {Tiny,Small,Medium,Large}
public enum GameDifficulty {Easy,Normal,Hard}
public enum UniversityLevel {BreakingGround,InitialExpansion,Stage1,Stage2,Stage3}

public class University
{
//CLASS VARS

	UniversitySize size;//----------classes of studentPopulation
	GameDifficulty difficulty;//----game difficulty, adjusts most parameters
	UniversityLevel level;//--------stage in game progression

	//double academicNotoriety;
	//double facultyQuality;
	//double academicBldgsQuality;

	double amenitiesQuantifier;

	int residentCapacity;
	double residentPopulation;//-------population of rent payers
	int studentCapacity;
	double studentPopulation;//--------population of tuition payers
	int activeAlumPopulation;//-----population of donating alums (grows as notoriety grows)

	double studentGrowthFactor;
	double studentGrowthRate;
	double residentGrowthFactor;
	double residentGrowthRate;
	double coffers;//---------------total player money in bank/wallet basic score
	double tuitionRate;//-----------tuition charged per student per update
	double rentRate;//--------------rent charged per resident per update
	double scholarshipAllocation;//-money allocated to scholarships (deducted from tuition revenue)
	double grantAllocation;//-------money allocated to research grants (deducted from tuition revenue)
	//double debt;//------------------total debt
	//double debtInterestRate;//------interest charged anually on debt

	double EASY_MOD;
	double NORM_MOD;
	double HARD_MOD;
	int MONTH;
	int YEAR;
    double DAY;
    double HOUR;
    long time;

//CONSTRUCTOR

	public University (GameDifficulty diff)
	{
		difficulty = diff;
		level = UniversityLevel.BreakingGround;
		size = UniversitySize.Tiny;
		EASY_MOD = 1.5;
		NORM_MOD = 1.0;
		HARD_MOD = 0.6;
        HOUR = 0.25;
        DAY = 24 * HOUR;
        MONTH = (int)(30 * DAY);
		YEAR = 12 * MONTH;

		double START_COFFERS = 150000.00;
		switch(diff)
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
		//academicNotoriety = 0;
		//facultyQuality = 0;
		//academicBldgsQuality = 0;

		residentCapacity = 0;
		residentPopulation = 0;
		studentCapacity = 0;
		studentPopulation = 0;
		activeAlumPopulation = 0;

		studentGrowthRate = 0.5; //Month
        studentGrowthFactor = 0;
		residentGrowthRate = 0.5; //Month
        residentGrowthFactor = 0;
		tuitionRate = 10000; //Year
		rentRate = 500; //Month
		scholarshipAllocation = 0; //Year
		grantAllocation = 0; //Year
                             //debt = 100000;
                             //debtInterestRate = 0.05 / YEAR;

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
            coffers = coffers + (tuitionRate / YEAR * studentPopulation) + (rentRate / MONTH * residentPopulation) - (scholarshipAllocation / YEAR) - (grantAllocation / YEAR);
        }
		switch(difficulty)
		{
		case GameDifficulty.Easy:
			coffers = coffers + activeAlumPopulation * 10 / MONTH;
			break;
		case GameDifficulty.Normal:
			coffers = coffers + activeAlumPopulation * 1 / MONTH;
			break;
		case GameDifficulty.Hard:
			coffers = coffers + activeAlumPopulation * 0.01 / MONTH;
			break;
		}
		Debug.Log ("Stud pop/cap: "+studentPopulation.ToString()+"/"+studentCapacity.ToString()+"   Res pop/cap: "+residentPopulation.ToString()+"/"+residentCapacity.ToString()+"    coffers: "+coffers.ToString());
	}

	public string HUD()
	{
        int studentPop = (int)studentPopulation;
        int residentPop = (int)residentPopulation;
        return "Time: " + Time() + "\nLevel: " + Level() + "\nMoney: $" + Coffers.ToString() + "\nStudents (POP/CAP): " + studentPop.ToString() + "/" + studentCapacity.ToString() + "\nResidents (POP/CAP): " + residentPop.ToString() + "/" + residentCapacity.ToString();
	}

    private void clock(){
        time++;
    }
    private string Level(){
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
                    //VICTORY
                } else{
                    Debug.Log("Requirements to advance not yet met");
                }break;
        }
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
//MUTATORS

	public int Coffers
	{
		get{ 
			return (int)coffers;	
		}
        set{
            coffers = value;
        }
   
	}

    public double ScholarshipAllocation
    {
        get{
            return scholarshipAllocation;
        }
        set{
            scholarshipAllocation = value;
        }
    }
    public double GrantAllocation
    {
        get{
            return grantAllocation;
        }
        set{
            grantAllocation = value;
        }
    }
    public double TuitionRate
    {
        get{
            return tuitionRate;
        }
    }
    public double RentRate
    {
        get{
            return rentRate;
        }
    }
}