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

		double START_COFFERS = 100000.00;
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
		residentPopulation = 2;
		studentCapacity = 0;
		studentPopulation = 2;
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
		if (studentCapacity != 0) {
            studentGrowthFactor = (studentGrowthRate / MONTH) * studentPopulation * ((studentCapacity - studentPopulation) / studentCapacity);
		}
		studentPopulation = studentPopulation + studentGrowthFactor;
		if (residentCapacity != 0) {
            residentGrowthFactor = (residentGrowthRate / MONTH) * residentPopulation * ((residentCapacity - residentPopulation) / residentCapacity);
		}
		residentPopulation = residentPopulation + residentGrowthFactor;
        coffers = coffers + (tuitionRate / YEAR * studentPopulation) + (rentRate / MONTH * residentPopulation) - (scholarshipAllocation / YEAR) - (grantAllocation / YEAR);

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
        return "Money: $" + Coffers.ToString() + "\nStudents (POP/CAP): " + studentPop.ToString() + "/" + studentCapacity.ToString() + "\nResidents (POP/CAP): " + residentPop.ToString() + "/" + residentCapacity.ToString() + "\nTime: " + Time();
	}

    private void clock(){
        time++;
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
        return "Year " + years.ToString() + ", Month " + months.ToString() + " Day " + days.ToString() + ", Hour " + hours.ToString();
    }
    public void addScholarship(double val)
    {
        scholarshipAllocation += val;
    }
    public void addGrant(double val)
    {
        grantAllocation += val;
    }
    public void addStudents(int val)
    {
        studentPopulation += val;
    }
    public void addResidents(int val)
    {
        residentPopulation += val;
    }   
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