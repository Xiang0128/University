using UnityEngine;

public static class GameData
{
    public static int selectedCourseId;
    public static string selectedCourseName;
    public static int selectedTestId;
    public static int userId;
    public static string targetPanelName = "";
    public static string selectedCameraName = "";
    public static WebCamTexture sharedWebcamTexture = null;

    public static string baseUrl = "https://sign.ngrok.pro";

    public static string predictUrl = baseUrl + "/api/predict";
    public static string loginUrl = baseUrl + "/login";
    public static string registerUrl = baseUrl + "/register";
    public static string getCoursesUrl = baseUrl + "/get_courses";
    public static string getWordsUrl = baseUrl + "/get_words/"; // + course_id
    public static string getSentencesUrl = baseUrl + "/get_sentences/"; // + course_id
    public static string getQuizWordsUrl = baseUrl + "/api/quiz_words/"; // + course_id
    public static string getFavoriteWordsUrl = baseUrl + "/get_favorite_words/"; // + user_id
    public static string isFavoritedUrl = baseUrl + "/is_favorited/"; // + user_id + "/" + model_id
    public static string favoriteWordUrl = baseUrl + "/favorite_word";
    public static string unfavoriteWordUrl = baseUrl + "/unfavorite_word";
    public static string unfavoriteWordFromVocabUrl = baseUrl + "/unfavorite_word_from_word";
    public static string updateProfileUrl = baseUrl + "/update_profile";
    public static string getUserQuizRecordsUrl = baseUrl + "/get_user_quiz_records";
    public static string getWrongWordsUrl = baseUrl + "/get_wrong_words_by_test_id";
    public static string saveWrongAnswersUrl = baseUrl + "/save_wrong_answers";
}
