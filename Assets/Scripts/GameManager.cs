using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class GameManager
{
    public static SaveData saveData;

    public enum Language
    {
        EN,
        CH
    }

    private static int LevelProgress;
    public static int levelProgress
    {
        set { LevelProgress = value; GameSave(); }
        get { return LevelProgress; }
    }
    private static float AudioVolume;
    public static float audioVolume
    {
        set { AudioVolume = value; GameSave(); }
        get { return AudioVolume; }
    }
    private static float SfxVolume;
    public static float sfxVolume
    {
        set { SfxVolume = value; GameSave(); }
        get { return SfxVolume; }
    }
    private static Language gameLanguage;
    public static Language language
    {
        set { gameLanguage = value; GameSave(); }
        get { return gameLanguage; }
    }

    static GameManager()
    {
        if (!File.Exists(Application.persistentDataPath + "/PTBSave.sav"))
        {
            levelProgress = 1;
            audioVolume = 0.6f;
            sfxVolume = 0.4f;
            language = Language.EN;
            Debug.Log("<�浵��ȡ>:�Ѵ����´浵.");
            return;
        }

        //ʹ���ļ��������л��ļ���Ϣ���������SaveData�������GameManager
        BinaryFormatter BF = new BinaryFormatter();
        FileStream FS = File.Open(Application.persistentDataPath + "/PTBSave.sav", FileMode.Open);
        saveData = BF.Deserialize(FS) as SaveData;
        FS.Close();
        LevelProgress = saveData.levelProgress;
        AudioVolume = saveData.audioVolume;
        SfxVolume = saveData.sfxVolume;
        gameLanguage = saveData.language;
        Debug.Log("<�浵��ȡ>:�浵��ȡ���.");
    }
    /*
    public static void SaveOnCurrent()
    {
    // �������ݱ�����ô˱��淽���������������
    }*/

    public static void SaveDataReset()
    {
        levelProgress = 1;
    }

    private static void GameSave()
    {
        if (saveData == null)
        {
            saveData = new SaveData(levelProgress, audioVolume, sfxVolume, language);
        }
        else saveData.GameSave(levelProgress, audioVolume, sfxVolume, language);

        //ʹ���ļ��������ļ���������װ����������л������ļ���
        BinaryFormatter BF = new BinaryFormatter();
        FileStream FS = File.Create(Application.persistentDataPath + "/PTBSave.sav");
        BF.Serialize(FS, saveData);
        FS.Close();
        Debug.Log("<��Ϸ����>:С�����Զ�����.");
    }
}

[System.Serializable]
public class SaveData
{
    public int levelProgress {private set; get; }
    public float audioVolume { private set; get; }
    public float sfxVolume { private set; get; }
    public GameManager.Language language { private set; get; }

    public SaveData(int levelProgress, float audioVolume, float sfxVolume, GameManager.Language language)
    {
        this.levelProgress = levelProgress;
        this.audioVolume = audioVolume;
        this.sfxVolume = sfxVolume;
        this.language = language;
    }

    public void GameSave(int levelProgress, float audioVolume, float sfxVolume, GameManager.Language language)
    {
        this.levelProgress = levelProgress;
        this.audioVolume = audioVolume;
        this.sfxVolume = sfxVolume;
        this.language = language;
    }
}
