using System;
using System.Collections.Generic;
using System.Data;
using Softscent.Models;

public partial class Pages_Custom : System.Web.UI.Page
{
    public List<Herb> HerbList = new List<Herb>();

    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = DataHelper.ExecuteQuery("SELECT * FROM Herbs");
        foreach (DataRow row in dt.Rows)
        {
            HerbList.Add(new Herb
            {
                Id = Convert.ToInt32(row["Id"]),
                Name = row["Name"].ToString(),
                Description = row["Description"] != DBNull.Value ? row["Description"].ToString() : "",
                Price = Convert.ToDecimal(row["Price"]),
                Benefit = row["Benefit"] != DBNull.Value ? row["Benefit"].ToString() : ""
            });
        }
        DataBind();
    }
    public string GetHerbThaiName(string name)
    {
        string n = name.ToLower();
        if (n.Contains("peppermint")) return "เปปเปอร์มิ้นท์";
        if (n.Contains("menthol")) return "เมนทอล";
        if (n.Contains("camphor")) return "พิมเสน/การบูร";
        if (n.Contains("eucalyptus")) return "ยูคาลิปตัส";
        if (n.Contains("lavender")) return "ลาเวนเดอร์";
        if (n.Contains("lemon") && !n.Contains("grass")) return "เลมอน";
        if (n.Contains("lemongrass")) return "ตะไคร้หอม";
        if (n.Contains("basil")) return "โหระพา";
        if (n.Contains("star anise") || n.Contains("poy kak")) return "โป๊ยกั๊ก";
        if (n.Contains("clove") || n.Contains("kan phlu")) return "กานพลู";
        if (n.Contains("cinnamon") || n.Contains("ob choey")) return "อบเชย";
        if (n.Contains("borneol") || n.Contains("phimsen")) return "พิมเสนเกล็ด";
        if (n.Contains("kaffir lime") || n.Contains("มะกรูด")) return "ผิวมะกรูด";
        if (n.Contains("citrus")) return "ส้มซ่า/มะนาว";
        if (n.Contains("bergamot")) return "มะกรูดฝรั่ง (เบอร์กามอท)";
        if (n.Contains("rosemary")) return "โรสแมรี่";
        if (n.Contains("nutmeg") || n.Contains("look jun")) return "ลูกจันทน์";
        if (n.Contains("ข่า") || n.Contains("galangal")) return "ข่า";

        return name;
    }

    public string GetHerbThaiBenefit(string name)
    {
        string n = name.ToLower();
        if (n.Contains("peppermint")) return "ช่วยให้เย็นสดชื่นทันที บรรเทาอาการคัดจมูก";
        if (n.Contains("menthol")) return "ช่วยผ่อนคลาย บรรเทาความเหนื่อยล้าสะสม";
        if (n.Contains("camphor")) return "ทำให้หายใจสะดวก แก้หน้ามืดตาลาย";
        if (n.Contains("eucalyptus")) return "บรรเทาอาการหวัด คัดจมูก และฆ่าเชื้อในระบบทางเดินหายใจ";
        if (n.Contains("lavender")) return "ช่วยให้ผ่อนคลาย หลับสบาย บรรเทาความเครียด";
        if (n.Contains("lemon") && !n.Contains("grass")) return "เพิ่มความสดชื่น กระตุ้นการทำงานของสมอง";
        if (n.Contains("lemongrass")) return "ช่วยให้จิตใจสงบ แก้ปวดศีรษะ";
        if (n.Contains("basil")) return "ช่วยระบบทางเดินหายใจ บรรเทาอาการไอ";
        if (n.Contains("star anise") || n.Contains("poy kak")) return "กลิ่นหอมอุ่น ช่วยขับเสมหะและแก้ไอ";
        if (n.Contains("clove") || n.Contains("kan phlu")) return "กลิ่นหอมเผ็ดร้อน ช่วยแก้ท้องอืดและบรรเทาอาการปวด";
        if (n.Contains("cinnamon") || n.Contains("ob choey")) return "ช่วยให้เลือดลมไหลเวียนดี กระตุ้นความจำ";
        if (n.Contains("borneol") || n.Contains("phimsen")) return "บำรุงหัวใจ แก้หน้ามืด และอาการวิงเวียน";
        if (n.Contains("kaffir lime") || n.Contains("มะกรูด")) return "กลิ่นหอมสดชื่น ช่วยให้จิตใจสงบและผ่อนคลาย";
        if (n.Contains("citrus")) return "กลิ่นหอมสดใส ช่วยให้รู้สึกกระปรี้กระเปร่า";
        if (n.Contains("bergamot")) return "ปรับสมดุลอารมณ์ ลดความกังวล";
        if (n.Contains("rosemary")) return "ช่วยให้มีสมาธิและจดจำได้ดีขึ้น";
        if (n.Contains("nutmeg") || n.Contains("look jun")) return "กลิ่นหอมหวาน ช่วยให้นอนหลับง่ายและผ่อนคลาย";
        if (n.Contains("ข่า") || n.Contains("galangal")) return "กลิ่นหอมร้อนแรง บรรเทาอาการจุกเสียดและทางเดินหายใจ";

        return "";
    }
}
