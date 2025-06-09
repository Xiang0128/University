package com.example.restaurant

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.Button
import android.widget.CheckBox
import android.widget.TextView
import android.widget.Toast

class MainActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        val dishes = listOf<CheckBox>(
            findViewById(R.id.dish1),
            findViewById(R.id.dish2),
            findViewById(R.id.dish3),
            findViewById(R.id.dish4),
            findViewById(R.id.dish5),
            // ...更多菜色
        )
        val checkoutButton = findViewById<Button>(R.id.checkoutButton)
        val resultTextView = findViewById<TextView>(R.id.resultTextView)

        checkoutButton.setOnClickListener {
            val selectedDishesCount = dishes.count { it.isChecked }

            when (selectedDishesCount) {
                3 -> {
                    resultTextView.text = "結帳金額：50 元"
                }
                4 -> {
                    resultTextView.text = "結帳金額：60 元"
                }
                else -> {
                    val message = if (selectedDishesCount < 3) "請至少選擇 3 樣菜" else "最多只能選擇 4 樣菜"
                    Toast.makeText(this, message, Toast.LENGTH_SHORT).show()
                    resultTextView.text = "" // 清空結果
                }
            }
        }
    }
}