/*#include "TradeManager.h"
#include "GameManager.h"
#include "CustomQueue.h"

JusickObject* JO = new JusickObject;
bool selecting = false, buying = false, selling = false, stateLog = false;
int number = 0;
int NmGGuVal = 1000, NmBVal = 2222, NmRVal = 3000, NmGGMVal = 10000;
int ChGGuVal = 0, ChBVal = 0, ChRVal = 0, ChGGmVal = 0;
//JusickData* JD = new JusickData;
//PlayerData* data = new PlayerData;

void SelectJusick(queue<string>& _playerLog, PlayerData& _data, JusickData& _JData)
{
    string JusickName;
    Key eKey = KeyController();

#pragma region 상호작용
    if (!selecting && !buying && !selling)
    {
        switch (eKey)
        {
            case Key::ONE:
                JusickName = "맨즈슈즈";
                number = 1;
                selecting = true;
                stateLog = true;
                break;
            case Key::TWO:
                JusickName = "비드제약";
                number = 2;
                stateLog = true;
                selecting = true;
                break;
            case Key::THREE:
                JusickName = "홍성회";
                number = 3;
                stateLog = true;
                selecting = true;
                break;
            case Key::FOUR:
                JusickName = "크레은행";
                number = 4;
                stateLog = true;
                selecting = true;
                break;
            case Key::FAIL:
                break;
        }
    }
    else if (selecting)
    {
        int stateNum = 1;
        switch (eKey)
        {
            case Key::Z:
                //cout << "Buy" << endl;
                PushQueue(_playerLog, "Buy");
                selecting = false;
                buying = true;
                stateLog = true;
                break;
            case Key::C:
                //cout << "Sell" << endl;
                PushQueue(_playerLog, "Sell");
                selecting = false;
                selling = true;
                stateLog = true;
                break;
            case Key::T:
                //cout << "취소" << endl;
                selecting = false;
                stateLog = true;
                break;
            case Key::FAIL:
                break;
        }
    }
    else if (buying)
    {
        switch (eKey)
        {
            case Key::Q:
                //cout << "1개 구매하셨습니다." << endl;
                if (_data.PlayerMoney - (ShowNmVal(number)) < 0)
                {
                    PushQueue(_playerLog, "돈이 부족합니다.");
                    buying = false;
                    break;
                }
                ChangePlayerData(number, _data, 1);
                JO->JusickBuy(number, 1);
                SetJusickData(number, true, ShowPChangeVal(number), SetJusickNmVal(number), _JData);
                PushQueue(_playerLog, "1개 구매하셨습니다.");
                SetJusickNmVal(number);
                buying = false;
                break;
            case Key::W:
                //cout << "5개 구매하셨습니다." << endl;
                if (_data.PlayerMoney - (ShowNmVal(number) * 5) < 0)
                {
                    PushQueue(_playerLog, "돈이 부족합니다.");
                    buying = false;
                    break;
                }
                ChangePlayerData(number, _data, 5);
                JO->JusickBuy(number, 5);
                PushQueue(_playerLog, "5개 구매하셨습니다.");
                SetJusickData(number, true, ShowPChangeVal(number), SetJusickNmVal(number), _JData);
                SetJusickNmVal(number);
                buying = false;
                break;
            case Key::E:
                //cout << "10개 구매하셨습니다." << endl;
                if (_data.PlayerMoney - (ShowNmVal(number) * 10) < 0)
                {
                    PushQueue(_playerLog, "돈이 부족합니다.");
                    buying = false;
                    break;
                }
                ChangePlayerData(number, _data, 10);
                JO->JusickBuy(number, 10);
                PushQueue(_playerLog, "10개 구매하셨습니다.");
                SetJusickData(number, true, ShowPChangeVal(number), SetJusickNmVal(number), _JData);
                SetJusickNmVal(number);
                buying = false;
                break;
            case Key::R:
                //cout << "50개 구매하셨습니다." << endl;
                if (_data.PlayerMoney - (ShowNmVal(number) * 50) < 0)
                {
                    PushQueue(_playerLog, "돈이 부족합니다.");
                    buying = false;
                    break;
                }
                ChangePlayerData(number, _data, 50);
                JO->JusickBuy(number, 50);
                PushQueue(_playerLog, "50개 구매하셨습니다.");
                SetJusickData(number, true, ShowPChangeVal(number), SetJusickNmVal(number), _JData);
                SetJusickNmVal(number);
                buying = false;
                break;
            case Key::T:
                //cout << "취소" << endl;
                stateLog = true;
                buying = false;
                break;
            case Key::FAIL:
                break;
        }
    }
    else if (selling)
    {
        switch (eKey)
        {
            case Key::Q:
                //cout << "1개 판매하셨습니다." << endl;
                if ((SetJusickVal(number, _data) - 1 < 0))
                {
                    PushQueue(_playerLog, "현재 개수가 부족합니다.");
                    selling = false;
                    break;
                }
                ChangePlayerData(number, _data, -1);
                SetJusickData(number, false, ShowMChangeVal(number), SetJusickNmVal(number), _JData);
                JO->JusickBuy(number, -1);
                SetJusickNmVal(number);
                PushQueue(_playerLog, "1개 판매하셨습니다.");
                selling = false;
                break;
            case Key::W:
                //cout << "5개 판매하셨습니다." << endl;
                if ((SetJusickVal(number, _data) - 5 < 0))
                {
                    PushQueue(_playerLog, "현재 개수가 부족합니다.");
                    selling = false;
                    break;
                }
                ChangePlayerData(number, _data, -5);
                SetJusickData(number, false, ShowMChangeVal(number), SetJusickNmVal(number), _JData);
                JO->JusickBuy(number, -5);
                SetJusickNmVal(number);
                PushQueue(_playerLog, "5개 판매하셨습니다.");
                selling = false;
                break;
            case Key::E:
                //cout << "10개 판매하셨습니다." << endl;
                if ((SetJusickVal(number, _data) - 1 < 10))
                {
                    PushQueue(_playerLog, "현재 개수가 부족합니다.");
                    selling = false;
                    break;
                }
                ChangePlayerData(number, _data, -10);
                SetJusickData(number, false, ShowMChangeVal(number), SetJusickNmVal(number), _JData);
                JO->JusickBuy(number, -10);
                SetJusickNmVal(number);
                PushQueue(_playerLog, "10개 판매하셨습니다.");
                selling = false;
                break;
            case Key::R:
                //cout << "50개 판매하셨습니다." << endl;
                if ((SetJusickVal(number, _data) - 1 < 50))
                {
                    PushQueue(_playerLog, "현재 개수가 부족합니다.");
                    selling = false;
                    break;
                }
                ChangePlayerData(number, _data, -50);
                SetJusickData(number, false, ShowMChangeVal(number), SetJusickNmVal(number), _JData);
                JO->JusickBuy(number, -50);
                SetJusickNmVal(number);
                PushQueue(_playerLog, "50개 판매하셨습니다.");
                selling = false;
                break;
            case Key::T:
                //cout << "취소" << endl;
                stateLog = true;
                selling = false;
                break;
            case Key::FAIL:
                break;
        }
    }

#pragma endregion
#pragma region 로그
    if (stateLog && selecting)
    {
        PushQueue(_playerLog, JusickName + "를 고르셨습니다.");
        PushQueue(_playerLog, "매수하시겠습니까? 매도하시겠습니까? (Z : 매수 / C : 매도 / T : 취소) : ");
        stateLog = false;
    }
    else if (stateLog && buying)
    {
        stateLog = false;
        PushQueue(_playerLog, "몇개 구매하시겠습니까? (Q : 1 / W : 5 / E : 10 / R : 50 / T : 취소) : ");
    }
    else if (stateLog && selling)
    {
        stateLog = false;
        PushQueue(_playerLog, "몇개 판매하시겠습니까? (Q : 1 / W : 5 / E : 10 / R : 50 / T : 취소) : ");
    }
    else if (!selecting && !buying && !selling && stateLog)
    {
        stateLog = false;
        PushQueue(_playerLog, "취소하셨습니다.");
    }

#pragma endregion


}

void SetJusickData(int num, bool upndown, int totalVal, int changedVal, JusickData& _Jdata)
{
    switch (num)
    {
        case 1:
            PushQueue(_Jdata.CsiJusickLogs, upndown, totalVal, changedVal, _Jdata.IsChange);
            break;
        case 2:
            PushQueue(_Jdata.BuchiJusickLogs, upndown, totalVal, changedVal, _Jdata.IsChange);
            break;
        case 3:
            PushQueue(_Jdata.ReGameJusickLogs, upndown, totalVal, changedVal, _Jdata.IsChange);
            break;
        case 4:
            PushQueue(_Jdata.GameMaGoJusickLogs, upndown, totalVal, changedVal, _Jdata.IsChange);
            break;
    }
}

int SetJusickNmVal(int num)
{
    int val = JO->ChangeJusick(num);
    int temp;
    if (val < 0) val = val * -1;

    switch (num)
    {
        case 1:
            temp = ChGGuVal;
            ChGGuVal = val;
            if (ChGGuVal - temp > 0)
                return ChGGuVal;
            else
                return -temp;
            break;
        case 2:
            temp = ChBVal;
            ChBVal = val;
            if (ChBVal - temp > 0)
                return ChBVal;
            else
                return -temp;
            break;
        case 3:
            temp = ChRVal;
            ChRVal = val;
            if (ChRVal - temp > 0)
                return ChRVal;
            else
                return -temp;
            break;
        case 4:
            temp = ChGGmVal;
            ChGGmVal = val;
            if (ChGGmVal - temp > 0)
                return ChGGmVal;
            else
                return -temp;
            break;
    }
}

int ShowPChangeVal(int num)
{
    int temp = JO->ChangeJusick(num);

    switch (num)
    {
        case 1:
            if (temp > 0)
                return NmGGuVal += temp;
            return NmGGuVal = 1000;
            break;
        case 2:
            if (temp != 0)
                return NmBVal += temp;
            return NmBVal = 2222;
            break;
        case 3:
            if (temp != 0)
                return NmRVal += temp;
            return NmRVal = 3000;
            break;
        case 4:
            if (temp != 0)
                return NmGGMVal += temp;
            return NmGGMVal = 1000;
            break;
    }
}

int ShowMChangeVal(int num)
{
    int temp = JO->ChangeJusick(num);

    switch (num)
    {
        case 1:
            if (temp != 0)
                return NmGGuVal -= temp;
            return NmGGuVal = 1000;
            break;
        case 2:
            if (temp != 0)
                return NmBVal -= temp;
            return NmBVal = 2222;
            break;
        case 3:
            if (temp != 0)
                return NmRVal -= temp;
            return NmRVal = 3000;
            break;
        case 4:
            if (temp != 0)
                return NmGGMVal -= temp;
            return NmGGMVal = 10000;
            break;
    }
}

void ChangePlayerData(int num, PlayerData& _data, int value)
{

    switch (num)
    {
        case 1:
            _data.havecsi += value;
            _data.PlayerMoney -= NmGGuVal * value;
            break;
        case 2:
            _data.haveBuch += value;
            _data.PlayerMoney -= NmBVal * value;
            break;
        case 3:
            _data.haveregame += value;
            _data.PlayerMoney -= NmRVal * value;
            break;
        case 4:
            _data.havegamemago += value;
            _data.PlayerMoney -= NmGGMVal * value;
            break;
    }
}

int ShowNmVal(int num)
{
    switch (num)
    {
        case 1:
            return NmGGuVal;
            break;
        case 2:
            return NmBVal;
            break;
        case 3:
            return NmRVal;
            break;
        case 4:
            return NmGGMVal;
            break;
    }
}

int SetJusickVal(int num, PlayerData& _data)
{
    switch (num)
    {
        case 1:
            return _data.havecsi;
            break;
        case 2:
            return _data.haveBuch;
            break;
        case 3:
            return _data.haveregame;
            break;
        case 4:
            return _data.havegamemago;
            break;
    }
}*/