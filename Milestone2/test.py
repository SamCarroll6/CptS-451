import json

def cleanStr4SQL(s):
    return s.replace("'", "`").replace("\n", " ")

def getval():
    with open('./JSONfiles/yelp_checkin.json', 'r') as f:
        line = f.readline()
        count_line = 0
        total = 0
        val = ""
        lines = 0
        days = 0
        while line:
                val = ""
                data = json.loads(line)
                business_id = str(cleanStr4SQL(data['business_id']))
                for day in data['time']:
                    for time in data['time'][day]:
                        total += data['time'][day][time]
                    days += 1
                    val += str(business_id) + " " + str(day) + " " + str(total) + " " + str(days) + " " + str(lines) + " "
                    total = 0
                print(val)
                lines += 1
                val = ""
                line = f.readline()

getval()