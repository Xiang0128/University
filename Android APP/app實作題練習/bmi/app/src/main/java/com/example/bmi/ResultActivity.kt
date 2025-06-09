package com.example.bmi

import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import android.widget.TextView

class ResultActivity : AppCompatActivity() {

    private lateinit var bmiResultTextView: TextView
    private lateinit var weightStatusTextView: TextView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_result)

        bmiResultTextView = findViewById(R.id.bmiResultTextView)
        weightStatusTextView = findViewById(R.id.weightStatusTextView)

        val height = intent.getDoubleExtra("height", 0.0)
        val weight = intent.getDoubleExtra("weight", 0.0)
        val gender = intent.getStringExtra("gender") ?: "Male"

        val bmi = weight / (height * height)
        val bmiResult = String.format("您的 BMI 值為: %.2f", bmi)
        bmiResultTextView.text = bmiResult

        // 根據性別判斷體重狀況
        val weightStatus = when (gender) {
            "Male" -> when {
                bmi < 20 -> "體重過輕"
                bmi <= 23 -> "體重適中"
                else -> "體重過重"
            }
            "Female" -> when {
                bmi < 18 -> "體重過輕"
                bmi <= 22 -> "體重適中"
                else -> "體重過重"
            }
            else -> "未知"
        }
        weightStatusTextView.text = "您的體重狀況為: $weightStatus"
    }
}
