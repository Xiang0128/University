package com.example.keyboard

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.Button
import android.widget.GridLayout
import android.widget.TextView


class MainActivity : AppCompatActivity() {

    private lateinit var inputTextView: TextView
    private lateinit var keypadGridLayout: GridLayout

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        inputTextView = findViewById(R.id.inputTextView)
        keypadGridLayout = findViewById(R.id.keypadGridLayout)

        for (i in 0 until keypadGridLayout.childCount) {
            val child = keypadGridLayout.getChildAt(i)
            if (child is Button) {
                child.setOnClickListener {
                    inputTextView.append(child.text)
                }
            }
        }
    }
}