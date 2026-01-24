using System;
using System.Collections.Generic;

public static class LangHelper
{
    private static Dictionary<string, string> _thaiStrings = new Dictionary<string, string>
    {
        { "CraftYourScent", "ปรุงยาดมในแบบของคุณ" },
        { "CraftSubtitle", "เลือกส่วนผสมสมุนไพรพรีเมียมเพื่อกลิ่นและสรรพคุณที่คุณต้องการ" },
        { "SelectIngredients", "เลือกส่วนผสม" },
        { "PremiumIngredients", "ส่วนผสมคุณภาพเยี่ยม" },
        { "ClassicHerbs", "สมุนไพรคลาสสิก" },
        { "ThaiHerbs", "สมุนไพรไทยพื้นบ้าน" },
        { "CreateBlend", "เพิ่มลงตะกร้า" }
    };

    public static string Get(string key)
    {
        if (_thaiStrings.ContainsKey(key))
            return _thaiStrings[key];
        return key;
    }
}
