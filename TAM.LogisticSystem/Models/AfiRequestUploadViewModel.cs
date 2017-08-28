//using system;
//using system.collections.generic;
//using system.componentmodel.dataannotations;
//using system.linq;
//using system.threading.tasks;
//using fluentvalidation;

//namespace tam.logisticsystem.models
//{
//    public class afirequestuploadviewmodel
//    {
//        public string framenumber { get; set; }
//        public string customername { get; set; }
//        public string ktp { get; set; }
//        public string address1 { get; set; }
//        public string address2 { get; set; }
//        public string address3 { get; set; }
//        public string province { get; set; }
//        public string city { get; set; }
//        public string postcode { get; set; }
//        public string regionafi { get; set; }
//        public datetimeoffset? effectivedate { get; set; }
//        public string color { get; set; }
//        public int vehicleid { get; set; }
//        public string branchcode { get; set; }
//        public string branchcodeafi { get; set; }
//        public string modelname { get; set; }
//        public string chassis { get; set; }
//        public string errordescription { get; set; }
//    }

//    public class afirequestuploadvalidator : abstractvalidator<afirequestuploadviewmodel>
//    {
//        static readonly afirequestuploadvalidator _instance = new afirequestuploadvalidator();
//        public afiuploadhashset afiupload { get; set; }
//        public static afirequestuploadvalidator instance
//        {
//            get
//            {
//                return _instance;
//            }
//        }
//        public afirequestuploadvalidator()
//        {
//            cascademode = cascademode.stoponfirstfailure;
//            rulefor(x => x.framenumber).notnull().withmessage("'frame number' harus diisi")
//                .maximumlength(30).withmessage("'frame number' tidak boleh lebih dari 30 karakter")
//                .must(validateframeisexists).withmessage("'frame number' tidak terdaftar")
//                .must(validateframeisapplied).withmessage("'frame number' sudah pernah diajukan")
//                .must(validateduplicateframe).withmessage("'frame number' tidak boleh duplikat");
//            rulefor(x => x.customername).notnull().withmessage("'nama customer' harus diisi")
//                .maximumlength(30).withmessage("'nama customer' tidak boleh lebih dari 30 karakter");
//            rulefor(x => x.ktp).notnull().withmessage("'no identitas' harus diisi")
//                .maximumlength(50).withmessage("'no identitas' tidak boleh lebih dari 50 karakter");
//            rulefor(x => x.address1).notnull().withmessage("'alamat1' harus diisi")
//                .maximumlength(30).withmessage("'alamat1' tidak boleh lebih dari 30 karakter");
//            rulefor(x => x.address2).notnull().withmessage("'alamat2' harus diisi")
//                .maximumlength(30).withmessage("'alamat2' tidak boleh lebih dari 30 karakter");
//            rulefor(x => x.address3).notnull().withmessage("'alamat3' harus diisi")
//                .maximumlength(30).withmessage("'alamat3' tidak boleh lebih dari 30 karakter");
//            rulefor(x => x.province).notnull().withmessage("'provinsi' harus diisi")
//                .maximumlength(20).withmessage("'provinsi' tidak boleh lebih dari 20 karakter")
//                .must(validateprovinsi).withmessage("'provinsi' tidak terdaftar");
//            rulefor(x => x.city).notnull().withmessage("'kota' harus diisi")
//                .maximumlength(30).withmessage("'kota' tidak boleh lebih dari 30 karakter")
//                .must(validatekota).withmessage("'kota' tidak terdaftar");
//            rulefor(x => x.postcode).notnull().withmessage("'kode pos' harus diisi")
//                .maximumlength(5).withmessage("'kode pos' tidak boleh lebih dari 5 karakter");
//            rulefor(x => x.regionafi).notnull().withmessage("'region afi' harus diisi")
//                .maximumlength(30).withmessage("'region afi' tidak boleh lebih dari 30 karakter")
//                .must(validateregionafi).withmessage("'region afi' tidak terdaftar");
//            rulefor(x => x.color).notnull().withmessage("'warna' harus diisi")
//                .maximumlength(20).withmessage("'warna' tidak boleh lebih dari 20 karakter");
//            rulefor(x => x.chassis).maximumlength(30).withmessage("'chassis' tidak boleh lebih dari 30 karakter");
//        }

//        public bool validateframeisapplied(string framenumber)
//        {
//            return !this.afiupload.appliedframeset.contains(framenumber);
//        }
//        public bool validateframeisexists(string framenumber)
//        {
//            return this.afiupload.existedframeset.contains(framenumber);
//        }

//        public bool validateprovinsi(string name)
//        {
//            return this.afiupload.provinsiset.contains(name);
//        }
//        public bool validatekota(string name)
//        {
//            return this.afiupload.kotaset.contains(name);
//        }
//        public bool validateregionafi(string region)
//        {
//            return this.afiupload.regionafiset.contains(region);
//        }
//        public bool validateduplicateframe(string framenumber)
//        {
//            return !this.afiupload.excelframelist.contains(framenumber.toupper());
//        }
//    }
//}
