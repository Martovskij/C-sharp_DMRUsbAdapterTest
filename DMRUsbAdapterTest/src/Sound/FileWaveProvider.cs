using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using log4net;
using log4net.Config;


namespace DMRUsbAdapterTest.src.Sound
{
    delegate void D_BufferedDataCallback(byte[] arg);

    class FileWaveProvider: NAudio.Wave.IWaveProvider
    {
        FileStream waveFileReader;
        public static readonly ILog log = LogManager.GetLogger(typeof(FileWaveProvider));
        WaveHeader waveHeader = null;
        D_BufferedDataCallback bufferedDataCallback = null;

        public FileWaveProvider(String pathToFile)
        {
           // try
           // {
                waveFileReader = new FileStream(pathToFile, FileMode.Open);
                byte[] header = new byte[44];
                waveFileReader.Read(header,0,44);
                waveHeader = new WaveHeader(header);
           // }
           // catch(IOException ex)
            //{
           //     log.Debug(ex.Message);
           // }
        }


        public void SetBufferedDataCallback(D_BufferedDataCallback new_bufferedDataCallback)
        {
            bufferedDataCallback = new_bufferedDataCallback;
        }

        public void CloseStream()
        {
            try
            {
                waveFileReader.Close();
            }
            catch(Exception ex)
            {
                log.Debug(ex.Message);
                log.Debug(ex.StackTrace);
            }

        }


        public NAudio.Wave.WaveFormat WaveFormat 
        { 
           get 
           {
                 try
                 {
                    if (waveHeader == null)
                        return new NAudio.Wave.WaveFormat(8000, 16, 1);
                    else return new NAudio.Wave.WaveFormat(waveHeader.SampleRate,waveHeader.BitsPerSample,waveHeader.NumChannels);
                 }
                 catch(Exception ex)
                 {
                    log.Debug(ex.Message );
                    return new NAudio.Wave.WaveFormat(8000, 16, 1);
                 }
           }

        }

        public int Read(byte[] buffer, int offset, int count)
        {
            try
            {
                int readCount = waveFileReader.Read(buffer, offset, count);
                if (bufferedDataCallback != null)
                {
                    byte[] newBuffer = (byte[])buffer.Clone();
                    bufferedDataCallback(newBuffer);
                }
                return readCount;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
           // finally{ waveFileReader.Close();}
            return -1;
        }

        
        public void CloseFile()
        {
            log.Info("close file");
            waveFileReader.Close();
        }

    }
}
