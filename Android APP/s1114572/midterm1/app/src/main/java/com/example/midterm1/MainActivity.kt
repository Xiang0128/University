package com.example.midterm1

import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.EditText
import android.widget.Spinner
import androidx.appcompat.app.AppCompatActivity

class MainActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        val predictButton = findViewById<Button>(R.id.predictButton)
        predictButton.setOnClickListener {
            // 獲取使用者輸入
            val age = findViewById<EditText>(R.id.ageEditText).text.toString().toIntOrNull() ?: 0
            val height = findViewById<EditText>(R.id.heightEditText).text.toString().toIntOrNull() ?: 0
            val weight = findViewById<EditText>(R.id.weightEditText).text.toString().toIntOrNull() ?: 0
            val frequency = findViewById<EditText>(R.id.frequencyEditText).text.toString().toIntOrNull() ?: 0
            val sportType = findViewById<Spinner>(R.id.sportTypeSpinner).selectedItem.toString()

            // 建立 Intent 並傳遞資料
            val intent = Intent(this, ResultActivity::class.java)
            intent.putExtra("age", age)
            intent.putExtra("height", height)
            intent.putExtra("weight", weight)
            intent.putExtra("frequency", frequency)
            intent.putExtra("sportType", sportType)
            startActivity(intent)
        }
    }
}