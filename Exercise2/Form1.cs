using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Exercise2;
using Microsoft.Win32;

namespace Exercise2
{
    enum Scheduler
    {
        EveryMinutes,
        EveryHour,
        EveryHalfDay,
        EveryDay,
        EveryWeek,
        EveryMonth,
        EveryYear,
    }
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            numHours.Value = DateTime.Now.Hour;
            numMins.Value = DateTime.Now.Minute;
        }
        CancellationTokenSource m_ctSource;

    
        private void startBtn_Click(object sender, EventArgs e)
        {
            numHours.Value = DateTime.Now.Hour;
            numMins.Value = DateTime.Now.Minute;
            if (m_ctSource != null)
            {
                m_ctSource.Cancel();
                prepareControlsForCancel();
            }
            int hour = (int)numHours.Value;
            int minutes = (int)numMins.Value;

            var dateNow = DateTime.Now;
            var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, hour, minutes, 0);
            string directory = Directory.GetCurrentDirectory();
            directory = directory + @"\\jsonJob.json";
            var json = File.ReadAllText(directory);
            List<Job> deserialisedJob = JsonConvert.DeserializeObject<List<Job>>(json);
            foreach (Job item in deserialisedJob)
                listBox1.Items.Add(item.Description);

            var nextDateValue = getNextDate(date, getScheduler());

            runCodeAt(nextDateValue, getScheduler());

        }



        private void runCodeAt(DateTime date, Scheduler scheduler)
        {
            m_ctSource = new CancellationTokenSource();

            var dateNow = DateTime.Now;
            TimeSpan ts;
            if (date > dateNow)
                ts = date - dateNow;
            else
            {
                date = getNextDate(date, scheduler);
                ts = date - dateNow;
            }

            prepareControlForStart();



            Task.Delay(ts).ContinueWith((x) =>
            {
                methodToCall(date);
                runCodeAt(getNextDate(date, scheduler), scheduler);
            }, m_ctSource.Token);
        }

       
        private void prepareControlForStart()
        {
            progressBar1.Enabled = true;
            progressBar1.Style = ProgressBarStyle.Marquee;
            button1.Enabled = false;
            button4.Enabled = false;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            button3.Enabled = true;
        }
       
        private void prepareControlsForCancel()
        {
            m_ctSource = null;
            progressBar1.Enabled = false;
            progressBar1.Style = ProgressBarStyle.Blocks;
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            button3.Enabled = false;
            button1.Enabled = true;
            button4.Enabled = true;
        }
      
        private DateTime getNextDate(DateTime date, Scheduler scheduler)
        {
            switch (scheduler)
            {
                case Scheduler.EveryMinutes:
                    return date.AddMinutes(1);
                case Scheduler.EveryHour:
                    return date.AddHours(1);
                case Scheduler.EveryHalfDay:
                    return date.AddHours(12);
                case Scheduler.EveryDay:
                    return date.AddDays(1);
                case Scheduler.EveryWeek:
                    return date.AddDays(7);
                case Scheduler.EveryMonth:
                    return date.AddMonths(1);
                case Scheduler.EveryYear:
                    return date.AddYears(1);
                default:
                    throw new Exception("Skedulues i gabuar");
            }

        }
  

        private void methodToCall(DateTime time)
        {
            //setup next call
            var nextTimeToCall = getNextDate(time, getScheduler());

            this.BeginInvoke((Action)(() =>
            {
                var strText = string.Format("Job eshte therritur {0}. Thirrja tjeter do jete ne : {1}", time, nextTimeToCall);
                listBox1.Items.Add(strText);
            }));



        }

      
        private Scheduler getScheduler()
        {
            if (radioButton1.Checked)
                return Scheduler.EveryMinutes;
            if (radioButton2.Checked)
                return Scheduler.EveryHour;

            if (radioButton4.Checked)
                return Scheduler.EveryDay;
            if (radioButton5.Checked)
                return Scheduler.EveryWeek;
            if (radioButton6.Checked)
                return Scheduler.EveryMonth;


            //default
            return Scheduler.EveryMinutes;
        }

        /// <summary>
        /// canceling the sheduler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelBtn_Click(object sender, EventArgs e)
        {
            if (m_ctSource != null)
            {
                m_ctSource.Cancel();
                prepareControlsForCancel();

                listBox1.Items.Add("Skeduluesi ndaloi!");
                numHours.Value = DateTime.Now.Hour;
                numMins.Value = DateTime.Now.Minute;
            }
            numHours.Value = DateTime.Now.Hour;
            numMins.Value = DateTime.Now.Minute;
        }


        /// <summary>
        /// Exits the app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitBtn_Click(object sender, EventArgs e)
        {
            if (progressBar1.Enabled)
            { MessageBox.Show("Skeduluesi eshte ne ekzekutim. Ndaloni skeduluesin pastaj mbylleni !"); }
            else
            {

                this.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            numHours.Value = DateTime.Now.Hour;
            numMins.Value = DateTime.Now.Minute;
            if (m_ctSource != null)
            {
                m_ctSource.Cancel();
                prepareControlsForCancel();
            }
                int hour = (int)numHours.Value;
            int minutes = (int)numMins.Value;

            var dateNow = DateTime.Now;
            var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, hour, minutes, 0);
            string directory = Directory.GetCurrentDirectory();
            directory = directory + @"\\XMLJob.xml";
            List<Job> jobs = new List<Job>();

            XmlDocument xml = new XmlDocument();
            xml.Load(directory);

            XmlNodeList xnList = xml.SelectNodes("/jobs/job");

            foreach (XmlNode xn in xnList)
            {
                Job job = new Job();
                job.Id = Convert.ToInt32(xn["id"].InnerText);
                job.Description = xn["description"].InnerText;
                jobs.Add(job);
            }






            foreach (Job item in jobs)
                listBox1.Items.Add(item.Description);
            var nextDateValue = getNextDate(date, getScheduler());
            runCodeAt(nextDateValue, getScheduler());
        }

      

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            if (progressBar1.Enabled)
            { MessageBox.Show("Skeduluesi eshte ne ekzekutim. Stoponi skeduluesin pastaj mbylleni !");
                e.Cancel=true;
            }
            else
            {

            }
        }
    }
}

