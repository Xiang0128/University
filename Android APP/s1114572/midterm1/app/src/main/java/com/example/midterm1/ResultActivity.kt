package com.example.midterm1

import android.os.Bundle
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity

class ResultActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_result)

        // 獲取傳遞的資料
        val age = intent.getIntExtra("age", 0)
        val height = intent.getIntExtra("height", 0)
        val weight = intent.getIntExtra("weight", 0)
        val frequency = intent.getIntExtra("frequency", 0)
        val sportType = intent.getStringExtra("sportType") ?: ""

        // 計算預測速度
        val predictedSpeed = when (sportType) {
            "跑步" -> 5 + (frequency * 0.3) - (age * 0.05) + (height.toDouble() / weight * 0.1)
            "游泳" -> 2 + (frequency * 0.2) - (age * 0.03) + (weight * 0.05)
            "騎自行車" -> 8 + (frequency * 0.4) - (age * 0.04) + (height * 0.02)
            else -> 0.0
        }

        // 提供建議
        val suggestion = when (sportType) {
            "跑步" -> when {
                predictedSpeed > 10 -> "表現優秀，建議參加比賽"
                predictedSpeed in 8.0..10.0 -> "運動表現良好，建議保持訓練"
                else -> "運動表現需要改善，建議增加運動頻率"
            }
            "游泳" -> when {
                predictedSpeed > 3 -> "表現優秀，建議參加比賽"
                predictedSpeed in 2.0..3.0 -> "運動表現良好，建議保持訓練"
                else -> "運動表現需要改善，建議增加運動頻率"
            }
            "騎自行車" -> when {
                predictedSpeed > 15 -> "表現優秀，建議參加比賽"
                predictedSpeed in 12.0..15.0 -> "運動表現良好，建議保持訓練"
                else -> "運動表現需要改善，建議增加運動頻率"
            }
            else -> ""
        }

        // 顯示結果
        val resultTextView = findViewById<TextView>(R.id.resultTextView)
        val suggestionTextView = findViewById<TextView>(R.id.suggestionTextView)

        resultTextView.text = "預測速度: %.2f 公里/小時".format(predictedSpeed)
        suggestionTextView.text = suggestion
    }
}