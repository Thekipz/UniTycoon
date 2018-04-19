using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum UniversitySize {Tiny,Small,Medium,Large}
public enum GameDifficulty {Easy,Normal,Hard}
public enum UniversityLevel {BreakingGround,InitialExpansion,Stage1,Stage2,Stage3}

public class University
{
	UniversitySize size;//----------classes of studentPopulation
	GameDifficulty difficulty;//----game difficulty, adjusts most parameters
	UniversityLevel level;//--------stage in game progression

	double academicNotoriety;
	double facultyQuality;
	double academicBldgsQuality;

	double amenitiesQuantifier;


	int residentPopulation;//-------population of rent payers
	int studentPopulation;//--------population of tuition payers
	int activeAlumPopulation;//-----population of donating alums (grows as notoriety grows)

	double coffers;//---------------total player money in bank/wallet basic score
	double tuitionRate;//-----------tuition charged per student per update
	double rentRate;//--------------rent charged per resident per update
	double scholarshipAllocation;//-money allocated to scholarships (deducted from tuition revenue)
	double grantAllocation;//-------money allocated to research grants (deducted from tuition revenue)
	double debt;//------------------total debt
	double debtInterestRate;//------interest charged anually on debt


	public University (GameDifficulty diff)
	{

	}
}