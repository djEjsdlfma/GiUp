/*#include "GameManager.h"

vector<Jusick*> JusickVector;


void JusickObject::Initialize()
{
    GGUshi* gj = new GGUshi;
    BBOchi* bj = new BBOchi;
    Ryhgyem* rj = new Ryhgyem;
    GGM* ggmj = new GGM;

    JusickVector.push_back(gj);
    JusickVector.push_back(bj);
    JusickVector.push_back(rj);
    JusickVector.push_back(ggmj);
}

void JusickObject::JusickBuy(int Index, int val)
{
    if (Index >= 0 && Index < JusickVector.size())
    {
        return JusickVector[Index - 1]->ChangeAmount(val);
    }
}

int JusickObject::ChangeJusick(int Index)
{
    if (Index >= 0 && Index < JusickVector.size())
    {
        return JusickVector[Index - 1]->ChangeValue();
    }
    else
        return 0;
}*/