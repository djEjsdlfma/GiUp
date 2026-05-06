/*#pragma once
#include<math.h>
#include<iostream>

using namespace std;

class Jusick
{
    public :
	 virtual int ChangeValue() const abstract;
	 virtual void ChangeAmount(int amount) const abstract;
};

class GGUshi : public Jusick
{
public:
	int ChangeValue() const override
	{
		value = (1000 - Jamount) * 12;
		return value;
	}
	void ChangeAmount(int amount) const override
	{
		Jamount -= amount;
	}

	mutable int value = 1000;
mutable int Jamount = 1000;
};

class BBOchi : public Jusick
{
public:
	int ChangeValue() const override
	{
		value = (1000 - Jamount) * 15;
return value;
	}
	void ChangeAmount(int amount) const override
	{
		Jamount -= amount;
	}

	mutable int value = 2222;
mutable int Jamount = 1000;
};

class Ryhgyem : public Jusick
{
public:
	void ChangeAmount(int amount) const override
	{
		Jamount -= amount;
	}
	int ChangeValue() const override
	{
		value = (1000 - Jamount) * 20;
return value;
	}

	mutable int value = 3000;
mutable int Jamount = 1000;
};

class GGM : public Jusick
{
public:
	int ChangeValue() const override
	{
		value = (1000 - Jamount) * 25;
return value;
	}
	void ChangeAmount(int amount) const override
	{
		Jamount -= amount;
	}

	mutable int value = 10000;
mutable int Jamount = 1000;
};*/