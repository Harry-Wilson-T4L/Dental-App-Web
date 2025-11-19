using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using DentalDrill.SmartFreight.Client.Models;
using NUnit.Framework;

namespace DentalDrill.SmartFreight.Client.Tests
{
    [TestFixture]
    public class ConsignmentTests
    {
        [Test]
        public void BasicConsignment()
        {
            var consignment = new Consignment
            {
                Receiver = new Receiver
                {
                    AccountNumber = "DDS",
                    Name = "Dental Drill Solutions",
                    Address = AddressDetails.AustralianAddress("PO Box 7300", null, "Baulkham Hills", "NSW", "2153"),
                },
                Sender = new Sender
                {
                    Name = "Annandale Dental",
                    Address = AddressDetails.AustralianAddress("Goodmans Building", "4 Johnston Street", "Annandale", "NSW", "2038"),
                },
                Carrier = Carrier.Automatic(),
                Lines = new List<FreightLine>
                {
                    new FreightLine
                    {
                        Amount = 1,
                        Description = "SATCHEL",
                        Weight = 2,
                    }
                }
            };

            var xml = consignment.SerializeToString();
            Assert.That(xml, Is.EqualTo(@"<connote>
  <recaccno>DDS</recaccno>
  <recname>Dental Drill Solutions</recname>
  <recaddr>
    <add1>PO Box 7300</add1>
    <add3>Baulkham Hills</add3>
    <add4>NSW</add4>
    <add5>2153</add5>
    <add6>Australia</add6>
  </recaddr>
  <sendname>Annandale Dental</sendname>
  <sendaddr>
    <add1>Goodmans Building</add1>
    <add2>4 Johnston Street</add2>
    <add3>Annandale</add3>
    <add4>NSW</add4>
    <add5>2038</add5>
    <add6>Australia</add6>
  </sendaddr>
  <carriername>[Automatic]</carriername>
  <freightlinedetails>
    <desc>SATCHEL</desc>
    <amt>1</amt>
    <wgt>2</wgt>
  </freightlinedetails>
</connote>"));
        }

        [Test]
        public void TestConsignment()
        {
            var consignment = new Consignment
            {
                ReturnConsignment = ConsignmentReturnType.Return,
                Receiver = new Receiver
                {
                    AccountNumber = "34282",
                    Name = "Maven Dental New Farm",
                    Address = new AddressDetails
                    {
                        AddressLine1 = "1/171 Moray Street",
                        Town = "NEW FARM",
                        State = "QLD",
                        Postcode = "4005",
                        Country = "AUSTRALIA",
                    },
                    ContactName = "Janet",
                    EmailAddress = "emails-test@devguild.ru",
                },
                Sender = new Sender
                {
                    Name = "DENTAL DRILL SOLUTIONS",
                    Address = new AddressDetails
                    {
                        AddressLine1 = "33 Benares Crescent",
                        Town = "ACACIA GARDENS",
                        State = "NSW",
                        Postcode = "2763",
                        Country = "AUSTRALIA"
                    },
                    EmailAddress = "emails-test@devguild.ru",
                },
                Carrier = new Carrier
                {
                    LeastCost = true,
                },
                WebPrint = true,
                Lines = new List<FreightLine>(),
            };

            consignment.Lines.Add(new FreightLine
            {
                Amount = 1,
                Description = "SATCHEL",
                Weight = 1,
                Volume = 0.001m,
            });

            consignment.SpecialInstructions = new SpecialInstructions
            {
                Line1 = "TEST CONSIGNMENT",
                Line2 = "PLEASE IGNORE",
            };

            var xml = consignment.SerializeToString();
            Assert.That(xml, Is.EqualTo(@"<connote>
  <returnconnote>Y</returnconnote>
  <recaccno>34282</recaccno>
  <recname>Maven Dental New Farm</recname>
  <reccontact>Janet</reccontact>
  <recemail>emails-test@devguild.ru</recemail>
  <recaddr>
    <add1>1/171 Moray Street</add1>
    <add3>NEW FARM</add3>
    <add4>QLD</add4>
    <add5>4005</add5>
    <add6>AUSTRALIA</add6>
  </recaddr>
  <sendname>DENTAL DRILL SOLUTIONS</sendname>
  <sendemail>emails-test@devguild.ru</sendemail>
  <sendaddr>
    <add1>33 Benares Crescent</add1>
    <add3>ACACIA GARDENS</add3>
    <add4>NSW</add4>
    <add5>2763</add5>
    <add6>AUSTRALIA</add6>
  </sendaddr>
  <applyleastcost>Yes</applyleastcost>
  <webprint>Yes</webprint>
  <freightlinedetails>
    <desc>SATCHEL</desc>
    <amt>1</amt>
    <wgt>1</wgt>
    <cube>0.001</cube>
  </freightlinedetails>
  <spins>
    <sp1>TEST CONSIGNMENT</sp1>
    <sp2>PLEASE IGNORE</sp2>
  </spins>
</connote>"));
        }

        [Test]
        public void DeserializeConsignment()
        {
            var xml = @"<Connote Entity=""KWN"">
    <_currency>$</_currency>
    <accno>10154871</accno>
    <adhocsend>Y</adhocsend>
    <allocatedconid>E62DFBD7-4D02-4E4A-A9B7-447C3C918899</allocatedconid>
    <applyleastcost>YES</applyleastcost>
    <applymarkup>Yes</applymarkup>
    <barcodetypeid>31</barcodetypeid>
    <bureauid>KWN</bureauid>
    <carrierid>227</carrierid>
    <carriername>STARTRACK EXP</carriername>
    <carrzone>BRS</carrzone>
    <chargeto>S</chargeto>
    <chgwgt>1</chgwgt>
    <co2offset>No</co2offset>
    <condate>17/05/2019</condate>
    <condatelegacy>79763</condatelegacy>
    <conno>K3AZ20000006</conno>
    <connotenumber>K3AZ20000006</connotenumber>
    <conprfx>K3AZ</conprfx>
    <consignmentdatenumber>20190517</consignmentdatenumber>
    <createdutcdatetime>1905170800</createdutcdatetime>
    <creatorid>IMPORTED</creatorid>
    <currency>AUD</currency>
    <dailyid>180</dailyid>
    <dailyid_wan>KWN180</dailyid_wan>
    <datelong>20190517</datelong>
    <doneloc>Y</doneloc>
    <freightlinedetails>
        <amt>1</amt>
        <cube>0.00100</cube>
        <defaultlabels>1</defaultlabels>
        <desc>SAT</desc>
        <wgt>1</wgt>
    </freightlinedetails>
    <fromzone>SYD</fromzone>
    <fromzoneheadport>SYD</fromzoneheadport>
    <grandtotmrkup>14.905</grandtotmrkup>
    <grandtotrate>14.905</grandtotrate>
    <gstamount>1.355</gstamount>
    <gstamt>1.355</gstamt>
    <gstonmrkup>1.355</gstonmrkup>
    <headport>BRS</headport>
    <identification>
        <conid>E62DFBD7-4D02-4E4A-A9B7-447C3C918899</conid>
    </identification>
    <importfilename>59B78DC9FF9845A7BF63</importfilename>
    <labelrangecurrentno>1AAB</labelrangecurrentno>
    <labelrangeendno>9ZZZ</labelrangeendno>
    <labelrangestartno>1AAA</labelrangestartno>
    <labels>
        <labelno_tracking>K3AZ20000006FPP00001</labelno_tracking>
    </labels>
    <labeltypeid>30</labeltypeid>
    <lastmodified>20190517080033</lastmodified>
    <leastcostapplied>Y</leastcostapplied>
    <mrkupchrg>0.00</mrkupchrg>
    <netcost>13.55</netcost>
    <newcon>true</newcon>
    <originaldespatchdate>20190517</originaldespatchdate>
    <printlabel>true</printlabel>
    <prntretcon>Y</prntretcon>
    <rate>13.55</rate>
    <rateerrorgeoloc>Unable to determine Geo Location</rateerrorgeoloc>
    <recaddr>
        <add1>33 BENARES CRESCENT</add1>
        <add3>ACACIA GARDENS</add3>
        <add4>NSW</add4>
        <add5>2763</add5>
        <add6>AUSTRALIA</add6>
    </recaddr>
    <recemail>emails-test@devguild.ru</recemail>
    <recloc>0,0</recloc>
    <recname>DENTAL DRILL SOLUTIONS</recname>
    <regaddr>
        <add1>33 BENARES CRESCENT</add1>
        <add3>ACACIA GARDENS</add3>
        <add4>NSW</add4>
        <add5>2763</add5>
        <add6>AUSTRALIA</add6>
    </regaddr>
    <regname>DENTAL DRILL SOLUTIONS</regname>
    <requested>
        <carriername>[Cheapest]</carriername>
        <service>[Cheapest]</service>
    </requested>
    <returnconnote>Y</returnconnote>
    <route>BRS|BNE|BNE|Y</route>
    <sendaccno>34282</sendaccno>
    <sendaddr>
        <add1>1/171 MORAY STREET</add1>
        <add3>NEW FARM</add3>
        <add4>QLD</add4>
        <add5>4005</add5>
        <add6>AUSTRALIA</add6>
    </sendaddr>
    <sendcontact>JANET</sendcontact>
    <sendemail>emails-test@devguild.ru</sendemail>
    <sendloc>0,0</sendloc>
    <sendname>MAVEN DENTAL NEW FARM</sendname>
    <service>FP PREMIUM</service>
    <servicecode>FPP</servicecode>
    <serviceid>722</serviceid>
    <spins>
        <sp1>TEST CONSIGNMENT</sp1>
        <sp2>PLEASE IGNORE</sp2>
    </spins>
    <startrackproductcode>FPP</startrackproductcode>
    <taxrate>10</taxrate>
    <totalcost>13.55</totalcost>
    <totcost>13.55</totcost>
    <totcostplusmrkup>13.55</totcostplusmrkup>
    <totcube>0.00100</totcube>
    <totdgcharge>0.00</totdgcharge>
    <totitems>1</totitems>
    <totwgt>1</totwgt>
    <trackingid>kWnjc4</trackingid>
    <webprint>YES</webprint>
    <zoneheadport>BRS</zoneheadport>
</Connote>";

            var consignment = Consignment.DeserializeFromString(xml);

            Assert.That(consignment.Date, Is.EqualTo(new DateTime(2019, 5, 17)));
            Assert.That(consignment.Prefix, Is.EqualTo("K3AZ"));
            Assert.That(consignment.Number, Is.EqualTo("K3AZ20000006"));
            Assert.That(consignment.Sender.Name, Is.EqualTo("MAVEN DENTAL NEW FARM"));
            Assert.That(consignment.Receiver.Name, Is.EqualTo("DENTAL DRILL SOLUTIONS"));
            Assert.That(consignment.RegisteredOwner.Name, Is.EqualTo("DENTAL DRILL SOLUTIONS"));
            Assert.That(consignment.ChargeTo, Is.EqualTo(ConsignmentChargeTo.Sender));
            Assert.That(consignment.TrackingId, Is.EqualTo("kWnjc4"));
            Assert.That(consignment.Totals.TotalCost, Is.EqualTo(13.55));
        }
    }
}
