package com.example.bmi

import android.content.Intent
import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import android.widget.Button
import android.widget.EditText
import android.widget.RadioGroup
import android.widget.Toast

class MainActivity : AppCompatActivity() {

    private lateinit var heightEditText: EditText
    private lateinit var weightEditText: EditText
    private lateinit var genderRadioGroup: RadioGroup
    private lateinit var calculateButton: Button

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        heightEditText = findViewById(R.id.heightEditText)
        weightEditText = findViewById(R.id.weightEditText)
        genderRadioGroup = findViewById(R.id.genderRadioGroup)
        calculateButton = findViewById(R.id.calculateButton)

        calculateButton.setOnClickListener {
            val heightStr = heightEditText.text.toString()
            val weightStr = weightEditText.text.toString()

            if (heightStr.isNotEmpty() && weightStr.isNotEmpty()) {
                val height = heightStr.toDouble() / 100 // 轉換為公尺
                val weight = weightStr.toDouble()

                val selectedGenderId = genderRadioGroup.checkedRadioButtonId
                val gender = if (selectedGenderId == R.id.maleRadioButton) "Male" else "Female"

                val intent = Intent(this, ResultActivity::class.java).apply {
                    putExtra("height", height)
                    putExtra("weight", weight)
                    putExtra("gender", gender)
                }
                startActivity(intent)
            } else {
                Toast.makeText(this, "請輸入身高和體重", Toast.LENGTH_SHORT).show()
            }
        }
    }
}
