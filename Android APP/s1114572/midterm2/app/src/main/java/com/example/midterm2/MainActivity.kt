package com.example.midterm2

import android.os.Bundle
import android.view.View
import android.widget.AdapterView
import android.widget.ArrayAdapter
import android.widget.ListView
import android.widget.Spinner
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity

class MainActivity : AppCompatActivity() {

    private val countryCityMap = hashMapOf(
        "台灣" to listOf("台北", "高雄", "台中"),
        "日本" to listOf("東京", "大阪", "京都"),
        "美國" to listOf("紐約", "洛杉磯", "舊金山"),
        "德國" to listOf("柏林", "慕尼黑", "法蘭克福")
    )

    private lateinit var countrySpinner: Spinner
    private lateinit var cityListView: ListView
    private lateinit var cityAdapter: ArrayAdapter<String>

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main) // 替換為你的佈局檔案名稱

        countrySpinner = findViewById(R.id.countrySpinner)
        cityListView = findViewById(R.id.cityListView)

        // 設定國家 Spinner
        val countries = countryCityMap.keys.toList()
        val countryAdapter = ArrayAdapter(this, android.R.layout.simple_spinner_item, countries)
        countryAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
        countrySpinner.adapter = countryAdapter

        // 初始化城市 ListView 的 Adapter
        cityAdapter = ArrayAdapter(this, android.R.layout.simple_list_item_1, emptyList())
        cityListView.adapter = cityAdapter

        // 設定 Spinner 選擇事件監聽器
        countrySpinner.onItemSelectedListener = object : AdapterView.OnItemSelectedListener {
            override fun onItemSelected(parent: AdapterView<*>?, view: View?, position: Int, id: Long) {
                val selectedCountry = countries[position]
                val cities = countryCityMap[selectedCountry] ?: emptyList()
                cityAdapter = ArrayAdapter(this@MainActivity, android.R.layout.simple_list_item_1, cities)
                cityListView.adapter = cityAdapter
            }

            override fun onNothingSelected(parent: AdapterView<*>?) {
                // Do nothing
            }
        }

        // 設定 ListView 點擊事件監聽器
        cityListView.onItemClickListener = AdapterView.OnItemClickListener { _, _, position, _ ->
            val selectedCity = cityAdapter.getItem(position) ?: ""
            Toast.makeText(this, selectedCity, Toast.LENGTH_SHORT).show()
        }
    }
}